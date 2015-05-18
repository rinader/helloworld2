using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Crossover.Builder.Server.Filters
{
    public class HttpsRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var context = actionContext.Request.GetOwinContext();
            if (!context.Request.IsSecure)
            {
                var response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
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