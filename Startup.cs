using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Logging;
using Microsoft.Owin.StaticFiles;
using Owin;


[assembly: OwinStartup(typeof(BombVacuum.Startup))]
namespace BombVacuum
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseErrorPage();

            #if DEBUG
            var options = new StaticFileOptions {FileSystem = new PhysicalFileSystem("../../")};
            app.UseStaticFiles(options);
            #endif
            #if !DEBUG
            var options = new StaticFileOptions {FileSystem = new PhysicalFileSystem("./")};
            app.UseStaticFiles(options);
            #endif

            app.MapSignalR();
            
            // Configure auth
            ConfigureAuth(app);

            ConfigureWebApi(app);

            app.UseNancy();

            
        }
    }
}
