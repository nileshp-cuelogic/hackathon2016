using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ErrorLoggerWebApp.Startup))]
namespace ErrorLoggerWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
