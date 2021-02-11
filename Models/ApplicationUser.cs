using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace AspNetIdentityApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegDate { get; set; }
        public DateTime? LastOnline { get; set; }
        public bool Checked { get; set; }
        public ApplicationUser()
        {
            Checked = false;
            LastOnline = null;
            RegDate = DateTime.Now;
        }
    }
}
