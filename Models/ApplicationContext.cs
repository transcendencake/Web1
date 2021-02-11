using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNetIdentityApp.Models
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext() : base("IdentityDB") { }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }
    }
}