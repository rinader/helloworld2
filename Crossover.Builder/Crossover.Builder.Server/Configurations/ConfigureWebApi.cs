using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Owin;

// ReSharper disable once CheckNamespace

namespace Crossover.Builder.Server
{
    public partial class Startup
    {
        public void ConfigureWebApi(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            app.UseWebApi(config);
        }
    }
}