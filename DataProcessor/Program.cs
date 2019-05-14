using log4net.Config;
using SimpleInjector;
using SolarApp.DataProcessor.Utility;
using SolarApp.DataProcessor.Utility.Classes;
using SolarApp.DataProcessor.Utility.Interfaces;
using SolarApp.Persistence;
using SolarApp.Utility.Classes;
using SolarApp.Utility.Interfaces;
using System.Threading;

namespace SolarApp.DataProcessor
{
	static class Program
    {
		private static Container container;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
			Bootstrap();
			XmlConfigurator.Configure();
#if DEBUG
			var service = new SolarAppService(
				container.GetInstance<IConfiguration>(), 
                container.GetInstance<IFileSystem>(), 
				container.GetInstance<IFtp>(),
				container.GetInstance<ILogger>(), 
                container.GetInstance<ISolarAppContext>(), 
                container.GetInstance<IServices>(), 
                container.GetInstance<ITimer>());
			service.Init();
			while (true)
			{
				Thread.Sleep(50000);
			}
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new SolarAppService(
                    container.GetInstance<IConfiguration>(), 
                    container.GetInstance<IFileSystem>(), 
				    container.GetInstance<IFtp>(), 
					container.GetInstance<ILogger>(),
                    container.GetInstance<ISolarAppContext>(), 
                    container.GetInstance<IServices>(), 
                    container.GetInstance<ITimer>()) 
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }

		private static void Bootstrap()
		{
			container = new Container();
			container.RegisterSingleton<IConfiguration, Configuration>();
			container.Register<IFileSystem, FileSystem>();
			container.Register<IFtp, Ftp>();
			container.Register<ILogger>(() => {
				return new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
				});
			container.Register<IServices, Services>();
			container.Register<ISolarAppContext, SolarAppContext>();
			container.Register<ITimer, ReliableTimer>();
			container.Verify();
		}
    }
}
