using Nancy;

namespace BombVacuum.Nancy.Modules
{
    public class HelloModule : NancyModule
    {
        public HelloModule()
        {
            Get["/"] = parameters => Response.AsFile("./assets/index.html", "text/html");

            Get["/{url*}"] = parameters => Response.AsFile("./assets/index.html", "text/html");
        }
    }
}