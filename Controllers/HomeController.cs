using AspNetIdentityApp.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using System.Web.Security;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext db = ApplicationContext.Create();

        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        private bool DeleteUser(ApplicationUser user)
        {
            if (user != null)
            {
                var userFound = UserManager.FindById(user.Id);
                if (HttpContext.User.Identity.GetUserId() == user.Id) HttpContext.GetOwinContext().Authentication.SignOut();
                var result = UserManager.Delete(userFound);
                return result.Succeeded;
            }
            return false;
        }
        private bool BlockUser(ApplicationUser user)
        {
            if (user != null)
            {
                UserManager.RemoveFromRole(user.Id, "user");
                var result = UserManager.AddToRole(user.Id, "blocked");
                return result.Succeeded;
            }
            return false;
        }
        private bool UnblockUser(ApplicationUser user)
        {
            if (user != null)
            {
                UserManager.RemoveFromRole(user.Id, "blocked");
                var result = UserManager.AddToRole(user.Id, "user");
                return result.Succeeded;
            }
            return false;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                if (UserManager.IsInRole(HttpContext.User.Identity.GetUserId(), "blocked"))
                {
                    return RedirectToAction("Logout", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("Logout", "Account");
            }
            return View(db.Users);
        }

        [HttpPost]
        [Authorize (Roles = "user")]
        public ActionResult Index(List<ApplicationUser> user)
        {
            foreach (var users in user)
            {
                db.Users.Find(users.Id).Checked = users.Checked;
                db.SaveChanges();
            }
            return View(db.Users);
        }

        [HttpPost]
        [Authorize (Roles = "user")]
        public ActionResult UserAction(string action, List<ApplicationUser> users)
        {
            try
            {
                if (UserManager.IsInRole(HttpContext.User.Identity.GetUserId(), "blocked"))
                {
                    return RedirectToAction("Logout", "Account");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("Logout", "Account");
            }            
             foreach (var user in users)
            {
                if (user.Checked)
                {
                    var currUser = db.Users.Find(user.Id);
                    switch (action)
                    {
                        case "block":
                            BlockUser(currUser);
                            break;

                        case "unblock":
                            UnblockUser(currUser);
                            break;

                        case "delete":
                            DeleteUser(currUser);
                            break;
                    }
                            
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}