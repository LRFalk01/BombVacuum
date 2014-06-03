using Microsoft.Owin;
using Owin;


[assembly: OwinStartup(typeof(BombVacuum.Startup))]
namespace BombVacuum
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure auth
            ConfigureAuth(app);

            ConfigureWebApi(app);

            app.UseNancy();

            
        }
    }
}
