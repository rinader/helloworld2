using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Crossover.Builder.Client.Providers;

namespace Crossover.Builder.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;

            Console.Write("Username: ");
            var userName = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();

            Run(userName, password).Wait();

            Console.WriteLine("Press <Enter> to exit...");
            Console.ReadLine();
        }

        static async Task Run(string userName, string password)
        {
            // Create an http client provider:
            var hostUriString = "https://localhost:9443/token";
            var provider = new OAuthClientProvider(hostUriString);
            
            try
            {
                // Pass in the credentials and retrieve a token dictionary:
                var tokenInfo = await provider.GetTokenInfo(userName, password);
                Console.WriteLine("Username: {0}, Token: {1}", tokenInfo.Username, tokenInfo.AccessToken);
            }
            catch (AggregateException ex)
            {
                // If it's an aggregate exception, an async error occurred:
                Console.WriteLine(ex.InnerException.Message);
            }
            catch (SecurityException ex)
            {
                // If it's an security exception:
                Console.WriteLine(ex.Message);
            }
        }
    }
}
