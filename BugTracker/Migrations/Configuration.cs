namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }

            if (!context.Roles.Any(r => r.Name == "Develooper"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(r => r.UserName == "Jecool17@gmail.com"))
            {
                ApplicationUser adminUser = new ApplicationUser()
                {
                    DisplayName = "Admin",
                    UserName = "Jecool17@gmail.com",
                    Email = "Jecool17@gmail.com",
                };

                userManager.Create(adminUser, "PassWord");
            }

            if (!context.Users.Any(r => r.UserName == "Cooley.e.James@gmail.com"))
            {
                ApplicationUser projMUser = new ApplicationUser()
                {
                    DisplayName = "Project Manager",
                    UserName = "Cooley.e.James@gmail.com",
                    Email = "Cooley.e.James@gmail.com",
                };

                userManager.Create(projMUser, "PassWord");
            }

            if (!context.Users.Any(r => r.UserName == "Developer@mailinator.com"))
            {
                ApplicationUser devUser = new ApplicationUser()
                {
                    DisplayName = "Developer",
                    UserName = "Developer@mailinator.com",
                    Email = "Developer@mailinator.com",
                };

                userManager.Create(devUser, "PassWord");
            }

            if (!context.Users.Any(r => r.UserName == "Submitter@mailinator.com"))
            {
                ApplicationUser subUser = new ApplicationUser()
                {
                    DisplayName = "Submitter",
                    UserName = "Submitter@mailinator.com",
                    Email = "Submitter@mailinator.com",
                };

                userManager.Create(subUser, "PassWord");
            }

            //Initialize

            ApplicationUser adU = context.Users.FirstOrDefault(r => r.Email == "Jecool17@gmail.com");
            if (adU != null)
            {
                userManager.AddToRole(adU.Id, "Admin");
            }

            ApplicationUser pmU = context.Users.FirstOrDefault(r => r.Email == "Cooley.e.James@gmail.com");
            if (pmU != null)
            {
                userManager.AddToRole(pmU.Id, "Project Manager");
            }

            ApplicationUser devU = context.Users.FirstOrDefault(r => r.Email == "Developer@mailinator.com");
            if (devU != null)
            {
                userManager.AddToRole(devU.Id, "Developer");
            }

            ApplicationUser subU = context.Users.FirstOrDefault(r => r.Email == "Submitter@mailinator.com");
            if (subU != null)
            {
                userManager.AddToRole(subU.Id, "Submitter");
            }

            //Seed name and descriptions tables
            //context.TicketTypes.AddOrUpdate(t => t.Name, new TicketType { Name = "", Description = "" },
            //                                             new TicketType { Name = "", Description = "" },
            //                                             new TicketType { Name = "", Description = "" },
            //                                             new TicketType { Name = "", Description = "" }


            // );

            //context.TicketStatuses.AddOrUpdate(t => t.Name, new TicketStatus { Name = "", Description = "" },
            //                                             new TicketStatus { Name = "", Description = "" },
            //                                             new TicketStatus { Name = "", Description = "" },
            //                                             new TicketStatus { Name = "", Description = "" }


            // );

            //context.TicketPriorities.AddOrUpdate(t => t.Name, new TicketPriority { Name = "", Description = "" },
            //                                             new TicketPriority { Name = "", Description = "" },
            //                                             new TicketPriority { Name = "", Description = "" },
            //                                             new TicketPriority { Name = "", Description = "" }


            // );

            context.Projects.AddOrUpdate(t => t.Name, new Project { Name = "Portfolio", Description = "This project is a collection of projects in the proccess of development. Currently has bootstrap excercises" },
                                                      new Project { Name = "Blog Site", Description = "This project is a display of blogs that accepts and allows users to comment on post. Admin of site can also create post. Displays the ability to use encapsulation,inheritance, interfaces and etc" },
                                                      new Project { Name = "BugTracker", Description = "This project is a display of all previous project skillset. " });




        }
    }
}
