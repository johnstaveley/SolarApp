[assembly: WebActivator.PostApplicationStartMethod(typeof(SolarApp.Web.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace SolarApp.Web.App_Start
{
    using System.Reflection;
    using System.Web.Mvc;

    using SimpleInjector;
    using SimpleInjector.Extensions;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using SolarApp.DataProcessor.Utility.Interfaces;
    using SolarApp.DataProcessor.Utility.Classes;
    using SolarApp.Persistence;
	using SolarApp.Utility.Interfaces;
	using SolarApp.Utility.Classes;
	using log4net.Config;
	using log4net;
    
    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            // Did you know the container can diagnose your configuration? 
            // Go to: https://simpleinjector.org/diagnostics
            var container = new Container();
            InitializeContainer(container);
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            // TODO: Fix this hack
            //container.Verify();
            
        }
     
        private static void InitializeContainer(Container container)
        {
            container.RegisterSingleton<IConfiguration, Configuration>();
            container.Register<IFileSystem, FileSystem>();
			container.Register<ILogger>(() =>
			{
				return new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
			});
            container.Register<IServices, Services>();
            container.Register<ISolarAppContext, SolarAppContext>();
            container.Register<ITimer, ReliableTimer>();

        }
    }
}