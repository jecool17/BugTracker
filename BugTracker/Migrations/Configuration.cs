namespace BugTracker.Migrations
{
    using BugTracker.Helpers;
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;

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
            if (!context.Roles.Any(r => r.Name == "MasterAdmin"))
            {
                roleManager.Create(new IdentityRole { Name = "MasterAdmin" });
            }

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            if (!context.Users.Any(r => r.UserName == "MasterAdmin@mailinator.com"))
            {
                ApplicationUser masterUser = new ApplicationUser()
                {
                    DisplayName = "Master",
                    UserName = "MasterAdmin@mailinator.com",
                    Email = "MasterAdmin@mailinator.com",
                };
                userManager.Create(masterUser, "PassW0rd");
            }


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
                    FirstName = "John",
                    LastName = "Singleton"
                };

                userManager.Create(subUser, "PassWord");
            }

            context.SaveChanges();
            //Initialize

            ApplicationUser maU = context.Users.FirstOrDefault(r => r.Email == "MasterAdmin@mailinator.com");
            if (maU != null)
            {
                userManager.AddToRole(maU.Id, "MasterAdmin");
            }

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


            context.SaveChanges();


            //demo users
            if (!context.Users.Any(r => r.UserName == "DemoAdmin@mailinator.com"))
            {
                ApplicationUser demoadminUser = new ApplicationUser()
                {
                    DisplayName = "DemoAdmin",
                    UserName = "DemoAdmin@mailinator.com",
                    Email = "DemoAdmin@mailinator.com",
                    AvatarURL = "/Uploads/Master-joda-icon.png"
                };

                userManager.Create(demoadminUser, "PassWord");
            }


            if (!context.Users.Any(r => r.UserName == "DemoProjectManager@mailinator.com"))
            {
                ApplicationUser demoprojectUser = new ApplicationUser()
                {
                    DisplayName = "DemoProjectManager",
                    UserName = "DemoProjectManager@mailinator.com",
                    Email = "DemoProjectManager@mailinator.com",
                    AvatarURL = "/Uploads/Profile-512.png"
                };

                userManager.Create(demoprojectUser, "PassWord");
            }

            if (!context.Users.Any(r => r.UserName == "DemoSub@mailinator.com"))
            {
                ApplicationUser demosubUser = new ApplicationUser()
                {
                    DisplayName = "DemoSubmitter",
                    UserName = "DemoSub@mailinator.com",
                    Email = "DemoSub@mailinator.com",
                    AvatarURL = "/Uploads/sub.png"
                };

                userManager.Create(demosubUser, "PassWord");
            }

            if (!context.Users.Any(r => r.UserName == "DemoDeveloper@mailinator.com"))
            {
                ApplicationUser demodevUser = new ApplicationUser()
                {
                    DisplayName = "DemoDeveloper",
                    UserName = "DemoDeveloper@mailinator.com",
                    Email = "DemoDeveloper@mailinator.com",
                    AvatarURL = "/Uploads/profile-picture-icon-11.jpg"
                };

                userManager.Create(demodevUser, "PassWord");
            }

            context.SaveChanges();

            //initialize demo users
            ApplicationUser demoadU = context.Users.FirstOrDefault(r => r.Email == "DemoAdmin@mailinator.com");
            if (demoadU != null)
            {
                userManager.AddToRole(demoadU.Id, "Admin");
            }

            ApplicationUser demopmU = context.Users.FirstOrDefault(r => r.Email == "DemoProjectManager@mailinator.com");
            if (demopmU != null)
            {
                userManager.AddToRole(demopmU.Id, "Project Manager");
            }

            ApplicationUser demosubU = context.Users.FirstOrDefault(r => r.Email == "DemoSub@mailinator.com");
            if (demosubU != null)
            {
                userManager.AddToRole(demosubU.Id, "Submitter");
            }

            ApplicationUser demodevU = context.Users.FirstOrDefault(r => r.Email == "DemoDeveloper@mailinator.com");
            if (demodevU != null)
            {
                userManager.AddToRole(demodevU.Id, "Developer");
            }

            context.SaveChanges();


            //Seed name and descriptions tables
            context.TicketTypes.AddOrUpdate(t => t.Name, new TicketType { Name = "Bug", Description = "An error has occured that resulted in either a database issue or file retrievment issue" },
                                                       new TicketType { Name = "Defect", Description = "An error has occured that resulted in either an display issue or presentation issue" },
                                                        new TicketType { Name = "Feature Request", Description = "A client has called requesting new features" },
                                                       new TicketType { Name = "Docuentation Request", Description = "A client has called requesting additonal documentation" },
                                                       new TicketType { Name = "Training Request", Description = "A client has called in to request a schedule training appointment" },
                                                       new TicketType { Name = "Complaint", Description = "A client has called in to make a general complaint" },
                                                        new TicketType { Name = "Other", Description = "A call has been recieved that requires prompt follow up" });

            context.SaveChanges();


            context.TicketStatuses.AddOrUpdate(t => t.Name, new TicketStatus { Name = "New / Unassigned", Description = "New ticket that has not been assigned", Value = 0 },
                                                         new TicketStatus { Name = "Assigned", Description = "The ticket has been assigned", Value = 15 },                                                         
                                                         new TicketStatus { Name = "Assigned / In Progress", Description = "Ticket that has been assigned and in progress", Value = 40 },
                                                        new TicketStatus { Name = "Resolved", Description = "Completed Ticket by assigned developer", Value = 75},
                                                         new TicketStatus { Name = "Archived", Description = "Ticket has been completed and approved by Manager", Value = 100 });


            context.SaveChanges();


            context.TicketPriorities.AddOrUpdate(t => t.Name, new TicketPriority { Name = "Low", Description = "Requires attention. Developers should complete if there are no Medium/High/Urgent priority tickets" },
                                                        new TicketPriority { Name = "Medium", Description = "Requires Normal attention. Developers should complete if there are no High/Urgent priority tickets" },
                                                         new TicketPriority { Name = "High", Description = "Requires Urgent attention. Developers should focus on completing before medium/low priority tickets" },
                                                         new TicketPriority { Name = "URGENT", Description = "Highest Demand. Developers should abandon all unfinish task and focus on current ticket" });




            context.Projects.AddOrUpdate(t => t.Name, new Project { Name = "Portfolio", Description = "This project is a collection of projects in the proccess of development. Currently has bootstrap excercises" },
                                                     new Project { Name = "Blog Site", Description = "This project is a display of blogs that accepts and allows users to comment on post. Admin of site can also create post. Displays the ability to use encapsulation,inheritance, interfaces and etc" },
                                                     new Project { Name = "BugTracker", Description = "This project is a display of all previous project skillset. " });

            context.SaveChanges();

            var blogSiteId = context.Projects.FirstOrDefault(p => p.Name == "Blog Site").Id;
            var portfolioId = context.Projects.FirstOrDefault(p => p.Name == "Portfolio").Id;
            var bugTradkerId = context.Projects.FirstOrDefault(p => p.Name == "BugTracker").Id;

            var projectHelper = new ProjectsHelper();

            projectHelper.AddUserToProject(demopmU.Id, blogSiteId);
            projectHelper.AddUserToProject(demodevU.Id, blogSiteId);
            projectHelper.AddUserToProject(demosubU.Id, blogSiteId);

            projectHelper.AddUserToProject(demopmU.Id, portfolioId);
            projectHelper.AddUserToProject(demodevU.Id, portfolioId);
            projectHelper.AddUserToProject(demosubU.Id, portfolioId);

            projectHelper.AddUserToProject(demopmU.Id, bugTradkerId);
            projectHelper.AddUserToProject(demodevU.Id, bugTradkerId);
            projectHelper.AddUserToProject(demosubU.Id, bugTradkerId);

            context.SaveChanges();


            context.Tickets.AddOrUpdate(
                p => p.Title,
                    new Ticket
                   {
                     ProjectId = blogSiteId,
                        OwnerUserId = demosubU.Id,
                        Title = "New Things",
                        Description = "Clients have requested that the blog site project have more functionality",
                        Created = DateTime.Now,
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
                        TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "New / Unassigned").Id,
                        TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Feature Request").Id,
                    },
                    new Ticket
                    {
                       ProjectId = portfolioId,
                        OwnerUserId = demosubU.Id,
                        AssignedToUserId = demodevU.Id,
                        Title = "Notify",
                        Description = "Clients have requested that the porfolio be updated with new projects",
                        Created = DateTime.Now,
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
                        TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "Assigned").Id,
                        TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Feature Request").Id,
                    },
                    new Ticket
                    {
                        ProjectId = bugTradkerId,
                      OwnerUserId = demosubU.Id,
                        AssignedToUserId = demodevU.Id,
                        Title = "Estimated Time of Publish?",
                        Description = "Clients Wants to be notified when Developers have finish the first version of the release",
                        Created = DateTime.Now,
                       TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "High").Id,
                        TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "Assigned / In Progress").Id,
                        TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Other").Id,

                    });



            context.SaveChanges();




        }
    }
}
