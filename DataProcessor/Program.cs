using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;
using DataProcessor.Utility;
using Persistence;

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
            ServiceBase[] ServicesToRun;
			Bootstrap();
            ServicesToRun = new ServiceBase[] 
            { 
                new SolarAppService(new ReliableTimer()) 
            };
            ServiceBase.Run(ServicesToRun);
        }

		private static void Bootstrap()
		{
			container = new Container();
			container.RegisterSingle<IConfiguration, Configuration>();
			container.Register<IFileSystem, FileSystem>();
			container.Register<ISolarAppContext, SolarAppContext>();
			container.Register<ITimer, ReliableTimer>();
			container.Verify();
		}
    }
}
