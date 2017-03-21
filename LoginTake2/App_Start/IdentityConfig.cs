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

namespace LoginTake2
{
    public class ApplicationUserManager
        : UserManager<ApplicationUser, string>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, string> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(
            IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(
                new UserStore<ApplicationUser, ApplicationRole, string,
                    ApplicationUserLogin, ApplicationUserRole,
                    ApplicationUserClaim>(context.Get<ApplicationDbContext>()));

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(
                        dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }


    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(
            IdentityFactoryOptions<ApplicationRoleManager> options,
            IOwinContext context)
        {
            return new ApplicationRoleManager(
                new ApplicationRoleStore(context.Get<ApplicationDbContext>()));
        }
    }


    // This is useful if you do not want to tear down the database each time you run the application.
    // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    // This example shows you how to create a new database if the Model changes
    public class ApplicationDbInitializer
        : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {
            var userManager = HttpContext.Current
                .GetOwinContext().GetUserManager<ApplicationUserManager>();

            var roleManager = HttpContext.Current
                .GetOwinContext().Get<ApplicationRoleManager>();

            // Initial Admin user:
            const string name = "Master";
            const string password = "Password1!";           
            const string roleName = "SuperAdmin";

            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = "master@gmail.com"};
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }

            //Create Role Admin if it does not exist
            var role2 = roleManager.FindByName("Admin");
            if (role2 == null)
            {
                role2 = new ApplicationRole("Admin");

                // Set the new custom property:
                var roleresult = roleManager.Create(role2);
            }

            //// Create Admin User:
            //var user = userManager.FindByName(name);
            //if (user == null)
            //{
            //    user = new ApplicationUser { UserName = name, Email = name };

            //    // Set the new custom properties:


            //    var result = userManager.Create(user, password);
            //    result = userManager.SetLockoutEnabled(user.Id, false);
            //}

            //// Add user admin to Role Admin if not already added
            //var rolesForUser = userManager.GetRoles(user.Id);
            //if (!rolesForUser.Contains(role.Name))
            //{
            //    userManager.AddToRole(user.Id, role.Name);
            //}

            //// Initial Vanilla User:
            //const string vanillaUserName = "Admin";
            //const string vanillaUserPassword = "Password";

            //// Add a plain vannilla Users Role:
            //const string usersRoleName = "Admin";


            ////Create Role Users if it does not exist
            //var usersRole = roleManager.FindByName(usersRoleName);
            //if (usersRole == null)
            //{
            //    usersRole = new ApplicationRole(usersRoleName);

            //    // Set the new custom property:

            //    var userRoleresult = roleManager.Create(usersRole);
            //}

            //// Create Vanilla User:
            //var vanillaUser = userManager.FindByName(vanillaUserName);
            //if (vanillaUser == null)
            //{
            //    vanillaUser = new ApplicationUser { UserName = vanillaUserName, Email = vanillaUserName };

            //    // Set the new custom properties:


            //    var result = userManager.Create(vanillaUser, vanillaUserPassword);
            //    result = userManager.SetLockoutEnabled(vanillaUser.Id, false);
            //}

            //// Add vanilla user to Role Users if not already added
            //var rolesForVanillaUser = userManager.GetRoles(vanillaUser.Id);
            //if (!rolesForVanillaUser.Contains(usersRole.Name))
            //{
            //    userManager.AddToRole(vanillaUser.Id, usersRole.Name);
            //}
        }
    }
}