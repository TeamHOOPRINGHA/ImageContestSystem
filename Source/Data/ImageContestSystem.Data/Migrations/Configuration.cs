namespace ImageContestSystem.Data.Migrations
{
    using Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ImageContestSystemDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ImageContestSystemDbContext context)
        {
            // if admin role doesn't exist, create it
            if (!context.Roles.Any(r => r.Name == "Administrator"))
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var role = new IdentityRole { Name = "Administrator" };

                roleManager.Create(role);
            }

            // if user doesn't exist, create one and add it to the admin role
            if (!context.Users.Any(u => u.UserName == resources.AdminName))
            {
                var userStore = new UserStore<User>(context);
                var userManager = new UserManager<User>(userStore);
                var user = new User { UserName = resources.AdminName, Email = resources.AdminEmail };

                userManager.Create(user, resources.AdminPassword);
                userManager.AddToRole(user.Id, "Administrator");
            }

            base.Seed(context);
        }
    }
}