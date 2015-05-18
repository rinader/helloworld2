using System;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Crossover.Builder.Server.Filters
{
    public class HttpsRedirectAttribute : ActionFilterAttribute
    {
        private readonly int _port = -1;

        public HttpsRedirectAttribute()
        {
        }

        public HttpsRedirectAttribute(int port)
        {
            this._port = port;
        }

        public HttpsRedirectAttribute(string portAppSettingsKey)
        {
            int port;
            if (int.TryParse(ConfigurationManager.AppSettings[portAppSettingsKey],
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out port)) this._port = port;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var context = actionContext.Request.GetOwinContext();
            if (!context.Request.IsSecure)
            {
                var location = new UriBuilder(actionContext.Request.RequestUri)
                {
                    Scheme = Uri.UriSchemeHttps,
                    Port = this._port
                }.ToString();

                var response = actionContext.Request.CreateResponse(HttpStatusCode.Redirect);
                response.Headers.Add("Location", location);
                actionContext.Response = response;
            }

            base.OnActionExecuting(actionContext);
        }

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            this.OnActionExecuting(actionContext);
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }
}