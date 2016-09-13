using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MPAS.Startup))]
namespace MPAS
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
