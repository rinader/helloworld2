using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Crossover.Builder.Server.Filters;
using Crossover.Builder.Server.Identity;
using Crossover.Builder.Server.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

namespace Crossover.Builder.Server.Controllers
{
    [Authorize]
    [HttpsRequired]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager)
        {
            this.UserManager = userManager;
        }

        private IAuthenticationManager Authentication
        {
            get { return this.Request.GetOwinContext().Authentication; }
        }

        public ApplicationUserManager UserManager
        {
            get { return this._userManager ?? this.Request.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { this._userManager = value; }
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = new ApplicationUser {UserName = userModel.UserName};
            var result = await this.UserManager.CreateAsync(user, userModel.Password);

            var errorResult = this.GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return this.Ok();
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            this.Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return this.Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._userManager != null)
                {
                    this._userManager.Dispose();
                    this._userManager = null;
                }
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return this.InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError("", error);
                    }
                }

                if (this.ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return this.BadRequest();
                }

                return this.BadRequest(this.ModelState);
            }

            return null;
        }
    }
}