using System;
using System.Web.Http;
using Crossover.Builder.Server.Filters;
using Microsoft.Owin;
using Owin;

namespace Crossover.Builder.Server
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            this.ConfigureAuth(appBuilder);
            this.ConfigureWebApi(appBuilder);

            appBuilder.UseErrorPage();
            appBuilder.Run(context =>
            {
                if (context.Request.Path.Equals(new PathString("/fail")))
                {
                    throw new Exception("Random exception");
                }

                if(context.Request.IsSecure)

                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Hello, world.");
            });
        }
    }
}