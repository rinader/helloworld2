using System;
using System.Configuration;
using System.Globalization;
using System.Net.Http;
using Microsoft.Owin.Hosting;

namespace Crossover.Builder.Server
{
    public class Program
    {
        private static void Main()
        {
            int httpPort;
            if (!int.TryParse(ConfigurationManager.AppSettings["http-port"],
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out httpPort)) httpPort = 80;

            int httpsPort;
            if (!int.TryParse(ConfigurationManager.AppSettings["https-port"],
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out httpsPort)) httpsPort = 443;

            var startOptions = new StartOptions
            {
                Urls =
                {
                    string.Format("http://+:{0}", httpPort),
                    string.Format("https://+:{0}", httpsPort)
                }
            };

            // Start OWIN host 
            using (WebApp.Start<Startup>(startOptions))
            {
                // Create HttpCient and make a request to api/values 
                //var client = new HttpClient();

                //var response = client.GetAsync(new UriBuilder {Scheme = "http", Port = httpPort} + "api/values").Result;

                //Console.WriteLine(response);
                //Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                Console.WriteLine("Press <Enter> to exit...");
                Console.ReadLine();
            }
        }
    }
}