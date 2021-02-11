using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetIdentityApp.Models
{
    public class RegisterModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 1)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(15,  MinimumLength = 1)]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}