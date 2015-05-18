using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crossover.Builder.Server.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;

namespace Crossover.Builder.Server.Providers
{
    public class ApplicationOAuthServerProvider
        : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(
            OAuthValidateClientAuthenticationContext context)
        {
            await Task.FromResult(context.Validated());
        }

        public override async Task GrantResourceOwnerCredentials(
            OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] {"*"});

            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            var user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null && context.UserName == "admin")
            {
                var result =
                    await userManager.CreateAsync(new ApplicationUser {UserName = context.UserName}, context.Password);
                if (!result.Succeeded)
                {
                    context.SetError("invalid_grant", result.Errors.Aggregate((e1, e2) => e1 + "," + e2));
                    return;
                }
                user = await userManager.FindAsync(context.UserName, context.Password);
            }
            
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
                OAuthDefaults.AuthenticationType);
            var cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);

            var properties = CreateProperties(user.UserName);
            var ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }
        
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                {"userName", userName}
            };
            return new AuthenticationProperties(data);
        }
    }
}