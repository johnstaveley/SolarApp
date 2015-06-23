using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SolarApp.Web.Startup))]
namespace SolarApp.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
