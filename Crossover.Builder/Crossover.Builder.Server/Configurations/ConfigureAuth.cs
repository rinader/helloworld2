using System;
using Crossover.Builder.Server.Data;
using Crossover.Builder.Server.Identity;
using Crossover.Builder.Server.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;

// ReSharper disable once CheckNamespace
namespace Crossover.Builder.Server
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(AuthContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/User/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager, DefaultAuthenticationTypes.ApplicationCookie))
                }
            });

            // Enable the application to use bearer token to store information for the signed in user
            app.UseCors(CorsOptions.AllowAll);
            app.UseOAuthBearerTokens(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),

                // TODO: For demo
                AllowInsecureHttp = true
            });
        }
    }
}