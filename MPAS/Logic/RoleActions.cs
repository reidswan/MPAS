using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MPAS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MPAS.Logic
{
    internal class RoleActions
    {
        internal void createAdmin()
        {
            // Access the application context and create result variables.
            Models.ApplicationDbContext context = new ApplicationDbContext();
            IdentityResult IdRoleResult;
            IdentityResult IdUserResult;

            // Create a RoleStore object by using the ApplicationDbContext object.
            // The RoleStore is only allowed to contain IdentityRole objects.
            var roleStore = new RoleStore<IdentityRole>(context);

            // Create a RoleManager object that is only allowed to contain IdentityRole objects.
            // When creating the RoleManager object, you pass in (as a parameter) a new RoleStore object.

            var roleMgr = new RoleManager<IdentityRole>(roleStore);
            
            // Then, you create the "Administrator" role if it doesn't already exist.
            if (!roleMgr.RoleExists("Administrator"))
            {
                IdRoleResult = roleMgr.Create(new IdentityRole("Administrator"));
                if (!IdRoleResult.Succeeded)
                {
                    // Handle the error condition if there's a problem creating the RoleManager object.
                }
            }

            // Create a UserManager object based on the UserStore object and the ApplicationDbContext object.
            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var appUser = new ApplicationUser()
            {
                UserName = "01360406",
            };
            IdUserResult = userMgr.Create(appUser, "Pa$$word");

            // If the new "Admin" user was successfully created,
            // add the "Admin" user to the "Administrator" role.

            if (IdUserResult.Succeeded)
            {
                IdUserResult = userMgr.AddToRole(appUser.Id, "Administrator");
                if (!IdUserResult.Succeeded)
                {
                    // Handle the error consdition if there's a problem adding the user to the role.
                }
            }
            else
            {
                // Handle the error condition if there's a problem creating the new user.
            }

        }
    }
}