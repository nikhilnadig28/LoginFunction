using LoginTake2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace LoginTake2.App_Start
{
    public class RoleChange :ApiController
    {
        public string RequestAdminAccess()
        {
            var userManager = HttpContext.Current
              .GetOwinContext().GetUserManager<ApplicationUserManager>();
            
            string user = User.Identity.GetUserId();
            string name = User.Identity.Name;
            if(userManager.GetRoles(user).Contains("SuperAdmin"))
            {
                return "Already Admin";
            }
            else
            {
                return "User: "+name+" would like to be admin";
            }

        }

        public bool MakeAdmin()
        {
            //var User = new ApplicationUser();
            var userManager = HttpContext.Current
                   .GetOwinContext().GetUserManager<ApplicationUserManager>();

            //var roleManager = HttpContext.Current
            //    .GetOwinContext().Get<ApplicationRoleManager>();

            //var user =  UserManager<.FindById(User.Identity.GetUserId());
            //var user = Membership.GetUser().ProviderUserKey.ToString();
            string user = User.Identity.GetUserId();

            //var user = (new ApplicationDbContext()).Users.FirstOrDefault(s => s.Id == userId);
            var roleName = "Admin";
            var rolesForUser = userManager.GetRoles(user);
            if (!rolesForUser.Contains(roleName))
            {
                var result = userManager.AddToRole(user, roleName);
                return true;
            }
            return false;
        }

        public void RemoveAdmin()
        {
            var userManager = HttpContext.Current
                   .GetOwinContext().GetUserManager<ApplicationUserManager>();

            string user = User.Identity.GetUserId();
            var roleName = "Admin";
            var rolesForUser = userManager.GetRoles(user);
            if (rolesForUser.Contains(roleName))
            {
                var result = userManager.RemoveFromRoles(user, roleName);
            }

        }
    }
}