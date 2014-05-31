using System.Linq;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
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
