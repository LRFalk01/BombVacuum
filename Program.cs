using System;
using Microsoft.Owin.Hosting;

namespace BombVacuum
{
    class Program
    {
        static void Main(string[] args)
        {
            #if DEBUG
            var url = "http://+:8888";
            #endif
            #if !DEBUG
            var url = "https://*:443";
            #endif

            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("Running on {0}", url);
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }
    }
}
