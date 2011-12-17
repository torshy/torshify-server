using System;
using System.Reflection;

namespace Torshify.Origo.Host
{
    class Program
    {
        #region Properties

        public static OrigoBootstrapper Bootstrapper
        {
            get;
            private set;
        }

        #endregion Properties

        #region Private Static Methods

        static void Main(string[] args)
        {
            InitializeAssemblyResolve();

            Bootstrapper = new OrigoBootstrapper();

            InitializeCommandLineOptions(args);

            Bootstrapper.Run();

            Console.ReadLine();
        }

        private static void InitializeAssemblyResolve()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                                                           {
                                                               String resourceName = "Torshify.Origo.Host.Dependencies." +
                                                                                     new AssemblyName(args.Name).Name + ".dll";

                                                               using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                                                               {
                                                                   Byte[] assemblyData = new Byte[stream.Length];
                                                                   stream.Read(assemblyData, 0, assemblyData.Length);
                                                                   return Assembly.Load(assemblyData);
                                                               }
                                                           };
        }

        private static void InitializeCommandLineOptions(string[] args)
        {
            bool showHelp = false;

            var p = new OptionSet
                        {
                            { "u|username=", "spotify username", userName => Bootstrapper.UserName = userName },
                            { "p|password=", "spotify password", password => Bootstrapper.Password = password },
                            { "httpPort=", "the port the http wcf services will be hosted on", (int port) => Bootstrapper.HttpPort = port },
                            { "tcpPort=", "the port the tcp wcf services will be hosted on", (int port) => Bootstrapper.TcpPort = port },
                            { "h|help",  "show this message and exit", v => showHelp = v != null }
                        };

            try
            {
                p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("greet: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `greet --help' for more information.");
            }

            if (showHelp)
            {
                p.WriteOptionDescriptions(Console.Out);
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        #endregion Private Static Methods
    }
}