using Nancy;

namespace BombVacuum.Nancy.Modules
{
    public class HelloModule : NancyModule
    {
        public HelloModule()
        {
            Get["/"] = parameters => Response.AsFile("./assets/index.html", "text/html");

            //this will only happen in dev. nginx captures requests to this directory in prod
            Get["/assets/{file*}"] = parameters =>
            {
                string file = "./assets/" + parameters["file"].Value;
                return Response.AsFile(file, "text/html");
            };
        }
    }
}