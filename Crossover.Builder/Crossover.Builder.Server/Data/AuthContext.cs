using Crossover.Builder.Server.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Crossover.Builder.Server.Data
{
    public class AuthContext : IdentityDbContext<ApplicationUser>
    {
        public AuthContext() : base("UsersDatabase")
        {
        }

        public static AuthContext Create()
        {
            return new AuthContext();
        }
    }
}