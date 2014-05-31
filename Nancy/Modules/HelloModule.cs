using Nancy;

namespace BombVacuum.Nancy.Modules
{
    public class HelloModule : NancyModule
    {
        public HelloModule()
        {
            Get["/"] = parameters => "Hello World";
        }
    }
}