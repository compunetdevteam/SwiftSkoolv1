using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SwiftSkoolv1.WebUI.Models;

namespace SwiftSkoolv1.WebUI.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SwiftSkoolv1.WebUI.Models.SwiftSkoolDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SwiftSkoolv1.WebUI.Models.SwiftSkoolDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            context.Configuration.AutoDetectChangesEnabled = false;

            context.Configuration.ValidateOnSaveEnabled = false;


            if (!context.Roles.Any(r => r.Name == "Student"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Student" };

                manager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "SuperAdmin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "SuperAdmin" };

                manager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "Teacher"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Teacher" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Form-Teacher"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Form-Teacher" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "Admin@Compunet.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "Admin@Compunet.com", Email = "Admin@Compunet.com" };

                manager.Create(user, "admin12345");
                manager.AddToRole(user.Id, "Admin");
            }
            if (!context.Users.Any(u => u.UserName == "SuperAdmin@Compunet.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "SuperAdmin@Compunet.com", Email = "SuperAdmin@Compunet.com" };

                manager.Create(user, "@superadmin12345");
                manager.AddToRole(user.Id, "SuperAdmin");
            }


            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
