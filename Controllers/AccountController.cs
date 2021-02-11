using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspNetIdentityApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication2.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            //if (RoleManager.Roles.Count() == 0)
            //{
            //    RoleManager.Create(new IdentityRole { Name = "user" });
            //    RoleManager.Create(new IdentityRole { Name = "blocked" });
            //}
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { UserName = model.Login, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "user");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(
                    model.Login, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid login or password");
                }
                else
                {
                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(
                        user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    user.LastOnline = DateTime.Now;
                    UserManager.Update(user);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }
    }
}
