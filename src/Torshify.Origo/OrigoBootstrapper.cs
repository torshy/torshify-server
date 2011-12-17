using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;

using AutoMapper;
using Torshify.Origo.Services.V1.Login;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

using Torshify.Origo.Audio;
using Torshify.Origo.Contracts.V1;
using Torshify.Origo.Contracts.V1.Query;
using Torshify.Origo.Extensions;
using Torshify.Origo.Interfaces;
using Torshify.Origo.Services.V1.Image;
using Torshify.Origo.Services.V1.Player;
using Torshify.Origo.Services.V1.Playlists;
using Torshify.Origo.Services.V1.Query;

using WcfContrib.Hosting;

namespace Torshify.Origo
{
    public class OrigoBootstrapper : MarshalByRefObject, IDisposable
    {
        #region Constructors

        public OrigoBootstrapper()
        {
            HttpPort = 1338;
            TcpPort = 1337;
        }

        #endregion Constructors

        #region Properties

        public int HttpPort
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public int TcpPort
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        protected IUnityContainer Container
        {
            get;
            private set;
        }

        protected ILog Logger
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            Container.Dispose();
        }

        public void Run()
        {
            InitializeLogging();
            InitializeContainer();
            InitializeSpotify();
            InitializeServices();
            InitializeAutoMapper();
            InitializeStartables();

            Logger.Info("torshify server initialized");
        }

        private ServiceHost CreateNetTcpServiceHost<T>(string name, Action<ServiceHost<T>> extraConfiguration = null)
        {
            ServiceHost<T> host = ServiceConfigurationDescription.Create(name)
                                    .WithNetTcp(TcpPort, KnownEndpointConfiguration.NetTcp, KnownSecurityMode.None)
                                    .MakeDiscoverable()
                                    .GenerateServiceHost<T>(h => h.ApplyBoosting = true);

            if (extraConfiguration != null)
            {
                extraConfiguration(host);
            }

            return host;
        }

        private ServiceHost CreateWebHttpServiceHost<T>(string name, Action<ServiceHost<T>> extraConfiguration = null)
        {
            var host = ServiceConfigurationDescription.Create(name)
                        .WithWebHttp(HttpPort, "web", KnownEndpointConfiguration.WebHttp, KnownSecurityMode.None)
                        .MakeDiscoverable()
                        .GenerateServiceHost<T>(h => h.ApplyBoosting = true);

            var endPoint = host.Description.Endpoints.FirstOrDefault(p => p.Binding.GetType() == typeof(WebHttpBinding));

            if (endPoint != null)
            {
                var webBehavior = endPoint.Behaviors.Find<WebHttpBehavior>();
                webBehavior.HelpEnabled = true;
            }

            if (extraConfiguration != null)
            {
                extraConfiguration(host);
            }

            return host;
        }

        private void InitializeAutoMapper()
        {
            Mapper.CreateMap<ILink, string>().ConvertUsing(link => { using (link)return link.AsUri(); });

            Mapper.CreateMap<IArtist, Artist>()
                .ForMember(dest => dest.PortraitID, opt => opt.MapFrom(src => src.PortraitId))
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => Mapper.Map<ILink, string>(src.ToLink())));

            Mapper.CreateMap<IAlbum, Album>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => Mapper.Map<ILink, string>(src.ToLink())))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Convert.ToString(src.Type)))
                .ForMember(dest => dest.CoverID, opt => opt.MapFrom(src => Convert.ToString(src.CoverId)));

            Mapper.CreateMap<ITrack, Track>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => Mapper.Map<ILink, string>(src.ToLink())))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration.TotalMilliseconds))
                .ForMember(dest => dest.OfflineStatus, opt => opt.MapFrom(src => Convert.ToString(src.OfflineStatus)));

            Mapper.CreateMap<IPlaylist, Playlist>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => Mapper.Map<ILink, string>(src.ToLink())))
                .ForMember(dest => dest.OfflineStatus, opt => opt.MapFrom(src => Convert.ToString(src.OfflineStatus)))
                .ForMember(dest => dest.ImageID, opt => opt.MapFrom(src => src.ImageId));

            Mapper.CreateMap<IArtistBrowse, ArtistBrowseResult>()
                .ForMember(dest => dest.BackendRequestDuration, opt => opt.MapFrom(src => src.BackendRequestDuration.TotalMilliseconds))
                .ForMember(dest => dest.Portraits, opt => opt.MapFrom(src => ToStringList(src.Portraits)));

            Mapper.CreateMap<IAlbumBrowse, AlbumBrowseResult>()
                .ForMember(dest => dest.BackendRequestDuration, opt => opt.MapFrom(src => src.BackendRequestDuration.TotalMilliseconds));

            Mapper.CreateMap<ISearch, QueryResult>();
        }

        private void InitializeContainer()
        {
            Container = new UnityContainer();
            Container.RegisterType<IPlaylistController, PlaylistController>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IMusicPlayer, NAudioMusicPlayer>(new ContainerControlledLifetimeManager());
            Container.RegisterStartable<IMusicPlayerController, MusicPlayerController>();
            Container.RegisterStartable<PlayerCallbackHandler, PlayerCallbackHandler>();
            Container.RegisterStartable<LoginCallbackHandler, LoginCallbackHandler>();
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(Container));
        }

        private void InitializeLogging()
        {
            var fileAppender = new RollingFileAppender();
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
            fileAppender.File = Path.Combine(Constants.LogFolder, assembly.GetName().Name + ".log");
            fileAppender.AppendToFile = true;
            fileAppender.MaxSizeRollBackups = 10;
            fileAppender.MaxFileSize = 1024 * 1024;
            fileAppender.RollingStyle = RollingFileAppender.RollingMode.Size;
            fileAppender.StaticLogFileName = true;
            fileAppender.Layout = new PatternLayout("%date{dd MMM yyyy HH:mm} [%thread] %-5level %logger - %message%newline");
            fileAppender.Threshold = Level.Info;
            fileAppender.ActivateOptions();

            var consoleAppender = new ColoredConsoleAppender();
            consoleAppender.AddMapping(
                new ColoredConsoleAppender.LevelColors
                {
                    ForeColor = ColoredConsoleAppender.Colors.White | ColoredConsoleAppender.Colors.HighIntensity,
                    BackColor = ColoredConsoleAppender.Colors.Red | ColoredConsoleAppender.Colors.HighIntensity,
                    Level = Level.Fatal
                });
            consoleAppender.AddMapping(
                new ColoredConsoleAppender.LevelColors
                {
                    ForeColor = ColoredConsoleAppender.Colors.Red | ColoredConsoleAppender.Colors.HighIntensity,
                    Level = Level.Error
                });
            consoleAppender.AddMapping(
                new ColoredConsoleAppender.LevelColors
                {
                    ForeColor = ColoredConsoleAppender.Colors.Yellow | ColoredConsoleAppender.Colors.HighIntensity,
                    Level = Level.Warn
                });
            consoleAppender.AddMapping(
                new ColoredConsoleAppender.LevelColors
                {
                    ForeColor = ColoredConsoleAppender.Colors.Green | ColoredConsoleAppender.Colors.HighIntensity,
                    Level = Level.Info
                });
            consoleAppender.AddMapping(
                new ColoredConsoleAppender.LevelColors
                {
                    ForeColor = ColoredConsoleAppender.Colors.White | ColoredConsoleAppender.Colors.HighIntensity,
                    Level = Level.Info
                });
            consoleAppender.Layout = new PatternLayout("%date{dd MM HH:mm} %-5level - %message%newline");
            #if DEBUG
            consoleAppender.Threshold = Level.All;
            #else
            consoleAppender.Threshold = Level.Info;
            #endif
            consoleAppender.ActivateOptions();

            Logger root;
            root = ((Hierarchy)LogManager.GetRepository()).Root;
            root.AddAppender(consoleAppender);
            root.AddAppender(fileAppender);
            root.Repository.Configured = true;

            Logger = LogManager.GetLogger("Bootstrapper");

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Exception exception = (Exception)e.ExceptionObject;
                Logger.Fatal(exception);
            };
        }

        private void InitializeServices()
        {
            ServiceHost[] hosts =
            {
                CreateNetTcpServiceHost<LoginService>("torshify/v1/login"),
                CreateNetTcpServiceHost<QueryService>("torshify/v1/query"),
                CreateNetTcpServiceHost<PlayerControlService>("torshify/v1/playercontrol"),
                CreateNetTcpServiceHost<TrackPlayerService>("torshify/v1/trackplayer"),
                CreateNetTcpServiceHost<PlaylistPlayerService>("torshify/v1/playlistplayer"),
                CreateNetTcpServiceHost<ImageService>("torshify/v1/image"),
                CreateWebHttpServiceHost<ImageService>("torshify/v1/image"),
                CreateNetTcpServiceHost<PlaylistService>("torshify/v1/playlist")
            };

            foreach (var host in hosts)
            {
                host.Open();

                foreach (var endpoint in host.Description.Endpoints)
                {
                    Logger.DebugFormat("{0}", endpoint.Address);
                }
            }
        }

        private void InitializeSpotify()
        {
            Directory.CreateDirectory(Constants.AppDataFolder);
            Directory.CreateDirectory(Constants.CacheFolder);
            Directory.CreateDirectory(Constants.SettingsFolder);

            ISession session =
                SessionFactory
                    .CreateSession(
                        Constants.ApplicationKey,
                        Constants.CacheFolder,
                        Constants.SettingsFolder,
                        Constants.UserAgent)
                    .SetPreferredBitrate(Bitrate.Bitrate320k);

            session.ConnectionError += (sender, e) => Logger.Debug(e.Status + " - " + e.Message);
            session.EndOfTrack += (sender, e) => Logger.Debug(e.Status + " - " + e.Message);
            session.LoginComplete += (sender, e) =>
                                         {
                                             if (e.Status != Error.OK)
                                             {
                                                 Logger.Fatal("Unable to log in to Spotify. " + e.Status.GetMessage());
                                                 Environment.Exit(-1);
                                             }
                                         };
            session.LogoutComplete += (sender, e) => Logger.Debug(e.Status + " - " + e.Message);
            session.MessageToUser += (sender, e) => Logger.Debug(e.Status + " - " + e.Message);
            session.OfflineStatusUpdated += (sender, e) =>
                                                {
                                                    var offlineStatus = session.GetOfflineSyncStatus();

                                                    Console.ForegroundColor = ConsoleColor.Cyan;

                                                    Console.WriteLine(
                                                        "Queued/Copied/Done : {0}/{1}/{2}",
                                                        offlineStatus.QueuedTracks,
                                                        offlineStatus.CopiedTracks,
                                                        offlineStatus.DoneTracks);
                                                    Console.WriteLine(
                                                        "Queued/Copied/Done : {0}/{1}/{2}",
                                                        (offlineStatus.QueuedBytes / (8 * 1024)),
                                                        (offlineStatus.CopiedBytes / (8 * 1024)),
                                                        (offlineStatus.DoneBytes / (8 * 1024)));

                                                    Console.WriteLine("Sync   : " + offlineStatus.IsSyncing);
                                                    Console.WriteLine("Errors : " + offlineStatus.ErrorTracks);
                                                    Console.WriteLine("NotCopy: " + offlineStatus.WillNotCopyTracks);

                                                    Console.ForegroundColor = ConsoleColor.Gray;
                                                };
            // Set up basic spotify logging
            session.LogMessage += (s, e) => Logger.Debug(e.Message);
            session.Exception += (s, e) => Logger.Error(e.Status.GetMessage() + " - " + e.Message);

            Container.RegisterInstance(session);

            Logger.Debug("Spotify session created");

            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                try
                {
                    session.Login(UserName, Password);
                }
                catch (Exception e)
                {
                    Logger.Fatal(e.Message);
                    Environment.Exit(-1);
                }
            }
        }

        private void InitializeStartables()
        {
            var startables = Container.ResolveAll<IStartable>();

            foreach (var startable in startables)
            {
                startable.Start();
            }
        }

        private IEnumerable<string> ToStringList(IEnumerable<IImage> portraits)
        {
            foreach (var portrait in portraits)
            {
                portrait.WaitForCompletion();

                using(portrait)
                {
                    yield return portrait.ImageId;
                }
            }
        }

        #endregion Methods
    }
}