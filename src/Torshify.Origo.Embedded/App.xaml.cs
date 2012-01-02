using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Torshify.Origo.Embedded
{
    public partial class App : Application
    {
        #region Methods

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            EmbeddOrigo();
        }

        private void EmbeddOrigo()
        {
            var args = Environment.GetCommandLineArgs();

            if (args.Length != 3)
            {
                MessageBox.Show("Please provide username and password as command line arguments");
                Environment.Exit(-1);
            }

            Task.Factory.StartNew(() =>
            {
                AppDomainSetup setup = new AppDomainSetup();
                setup.ApplicationName = "Spotify";
                setup.ApplicationBase = Environment.CurrentDirectory;

                AppDomain origoDomain = AppDomain.CreateDomain("OrigoDomain", null, setup);
                AppDomain.CurrentDomain.AssemblyResolve += OrigoDomainOnAssemblyResolve;
                OrigoBootstrapper host = origoDomain.CreateInstanceAndUnwrap(
                    typeof(OrigoBootstrapper).Assembly.FullName,
                    "Torshify.Origo.OrigoBootstrapper") as OrigoBootstrapper;
                AppDomain.CurrentDomain.AssemblyResolve -= OrigoDomainOnAssemblyResolve;

                if (host != null)
                {
                    host.Run();
                }
            });
        }

        private Assembly OrigoDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                Assembly assembly = Assembly.Load(args.Name);
                if (assembly != null)
                    return assembly;
            }
            catch
            {
                // ignore load error
            }

            // *** Try to load by filename - split out the filename of the full assembly name
            // *** and append the base path of the original assembly (ie. look in the same dir)
            // *** NOTE: this doesn't account for special search paths but then that never
            //           worked before either.
            string[] parts = args.Name.Split(',');
            string file = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + parts[0].Trim() +
                          ".dll";
            if (File.Exists(file))
            {
                return Assembly.LoadFrom(file);
            }

            return null;
        }

        #endregion Methods
    }
}