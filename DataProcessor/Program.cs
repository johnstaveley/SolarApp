using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;
using DataProcessor.Utility;
using DataProcessor.Utility.Interfaces;
using DataProcessor.Utility.Classes;
using Persistence;
using System.Threading;

namespace DataProcessor
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
#if DEBUG
			var service = new SolarAppService(container.GetInstance<IConfiguration>(), container.GetInstance<IFileSystem>(), 
				container.GetInstance<IFtp>(), container.GetInstance<ISolarAppContext>(), container.GetInstance<ITimer>());
			service.Init();
			while (true)
			{
				Thread.Sleep(50000);
			}
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new SolarAppService(new ReliableTimer()) 
            };
            ServiceBase.Run(ServicesToRun);
#endif
		}

		private static void Bootstrap()
		{
			container = new Container();
			container.RegisterSingle<IConfiguration, Configuration>();
			container.Register<IFileSystem, FileSystem>();
			container.Register<IFtp, Ftp>();
			container.Register<ISolarAppContext, SolarAppContext>();
			container.Register<ITimer, ReliableTimer>();
			container.Verify();
		}
    }
}
