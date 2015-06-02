using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Web;
using ContestsPortal.Domain.DataAccess;
using ContestsPortal.Domain.Models;
using ContestsPortal.WebSite.App_Start;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MenuItem = ContestsPortal.Domain.Models.MenuItem;
using System;

namespace ContestsPortal.WebSite
{
    public class CustomDropCreateDbAlways : DropCreateDatabaseAlways<PortalContext>
    {
        protected override void Seed(PortalContext context)
        {
            Debug.WriteLine("In CustomDropCreateDbAlways.");
            DbSeeding.DoInitialization(context);
            base.Seed(context);
        }
    }

    public class CustomDropCreateDbIfModelChanges : DropCreateDatabaseIfModelChanges<PortalContext>
    {
        protected override void Seed(PortalContext context)
        {
            Debug.WriteLine("In CustomDropCreateIfModelChanges.");
            DbSeeding.DoInitialization(context);
            base.Seed(context);
        }
    }


    public class CustomCreateDbIfNotExist : CreateDatabaseIfNotExists<PortalContext>
    {
        protected override void Seed(PortalContext context)
        {
            Debug.WriteLine("In CustomCreateDbIfNotExist.");
            DbSeeding.DoInitialization(context);
            base.Seed(context);
        }
    }

    

    internal static class DbSeeding
    {
        public static void DoInitialization(PortalContext context)
        {
            Debug.WriteLine("Begin seeding.");
            InitTaskCategories(context);
            InitContestPriorities(context);
            InitContestStates(context);
            InitMenuItemCategories(context);
            InitMenuItems(context);
            InitProgrammingLanguages(context);
            InitFeedbackTypes(context);
            InitCountries(context);
            InitRolesAndAdminProfile(context);
            Debug.WriteLine("End seeding.");
        }

         

        private static void InitContestPriorities(PortalContext context)
        {
            var priorities = new[]
            {
              new ContestPriority(){ContestPriorityName = "Олимпиада"},
              new ContestPriority(){ContestPriorityName = "Ежемесячный турнир"},
              new ContestPriority(){ContestPriorityName = "Еженедельный турнир"},
              new ContestPriority(){ContestPriorityName = "Тренировка"}
            };
            context.Set<ContestPriority>().AddOrUpdate(x=>x.ContestPriorityName,priorities);
        }
        

        private static void InitContestStates(PortalContext context)
        {
            var states = new[]
            {
              new ContestState(){ContestStateName = ContestStates.Awaiting, ContestStateComment = CommonResources.ContestStateAwaiting},
              new ContestState(){ContestStateName = ContestStates.Registration, ContestStateComment = CommonResources.ContestStateRegistration},
              new ContestState(){ContestStateName = ContestStates.Active, ContestStateComment = CommonResources.ContestStateActive},
              new ContestState(){ContestStateName = ContestStates.ResultsEvaluation, ContestStateComment = CommonResources.ContestStateResultEvaluation},
              new ContestState(){ContestStateName = ContestStates.Completed, ContestStateComment = CommonResources.ContestStateCompleted}
            };
            context.Set<ContestState>().AddOrUpdate(x=>x.ContestStateName,states);
        }


        private static void InitRolesAndAdminProfile(PortalContext context)
        {
            var currentcontext = HttpContext.Current;
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();

            const string name = "webadmin";
            const string email = "webadmin@mail.ru";
            const string password = "Keyword1";
            var rolenames = new[]
            {
                Roles.SuperAdministrator,
                Roles.Administrator,
                Roles.Moderator,
                Roles.Judge,
                Roles.Competitor,
                Roles.Member
            };

          foreach (string rolename in rolenames)
            {
                CustomIdentityRole role = roleManager.FindByName(rolename);
                if (role == null)
                {
                    role = new CustomIdentityRole { Name = rolename };
                    IdentityResult roleresult = roleManager.Create(role);
                }
            }

            var country = context.Countries.Single(x => x.CountryName == "Russia");
            UserProfile user = userManager.FindByName(name);
            if (user == null)
            {
                user = new UserProfile { UserName = name, Email = email, NickName = name, IdCountry = country.CountryId};
                IdentityResult result = userManager.Create(user, password);
            }


            IList<string> rolesForUser = userManager.GetRoles(user.Id);
            CustomIdentityRole adminrole = roleManager.FindByName(rolenames.First());
            if (!rolesForUser.Contains(adminrole.Name))
            {
                IdentityResult result = userManager.AddToRole(user.Id, adminrole.Name);
            }
        }
        
        private static void InitCountries(PortalContext context)
        {
            Debug.WriteLine("Begin init countries.");
            context.Countries.AddRange(new[]
            {
                new Country {CountryName = "Undefined"},
                new Country {CountryName = "USA"},
                new Country {CountryName = "Russia"},
                new Country {CountryName = "Ukraine"},
                new Country {CountryName = "Belorussia"},
                new Country {CountryName = "Georgia"},
                new Country {CountryName = "Kazahstan"},
                new Country {CountryName = "Nigeria"}
            });
            context.SaveChanges();
            Debug.WriteLine("End init countries.");
        }

        private static void InitTaskCategories(PortalContext context)
        {
            /*context.Set<TaskCategory>().AddOrUpdate(x => x.CategoryName, new[]
            { new TaskCategory() {CategoryName = }});*/

        }


        private static void InitProgrammingLanguages(PortalContext context)
        {
            context.Set<ProgrammingLanguage>().AddOrUpdate(x=>x.LanguageName,new []
            {   
                new ProgrammingLanguage("C++"),
                new ProgrammingLanguage("C#"),
                new ProgrammingLanguage("F#"),
                new ProgrammingLanguage("PHP"),
                new ProgrammingLanguage("Javascript"),
                new ProgrammingLanguage("Java"),
                new ProgrammingLanguage("Ruby"),
                new ProgrammingLanguage("Python")
            });
        }


        private static void InitMenuItems(PortalContext context)
        {
            var items = new[]
            {
                new MenuItem {MenuItemTitle = "Главная", ControllerName = "Home", ActionName = "Index", OrderNumber = 0, MinimalAccessibleRole = Roles.Guest},
                new MenuItem {MenuItemTitle = "Форум", ControllerName = "Member", ActionName = "Forum", OrderNumber = 2, MinimalAccessibleRole = Roles.Guest},
                new MenuItem {MenuItemTitle = "Управление", ControllerName = "Administrator", ActionName = "Index", OrderNumber = 1, MinimalAccessibleRole = Roles.Administrator},
                 new MenuItem {MenuItemTitle = "Профиль", ControllerName = "Member", ActionName = "Index", OrderNumber = 1, MinimalAccessibleRole = Roles.Member},
                new MenuItem {MenuItemTitle = "Соревнования", ControllerName = "Home", ActionName = "ContestsHistory", OrderNumber = 3, MinimalAccessibleRole = Roles.Guest},
                new MenuItem {MenuItemTitle = "Архив задач", ControllerName = "Home", ActionName = "ArchivedTasks", OrderNumber = 4, MinimalAccessibleRole = Roles.Guest},
                new MenuItem {MenuItemTitle = "Рейтинг", ControllerName = "Home", ActionName = "CompetitorsRating", OrderNumber = 5, MinimalAccessibleRole = Roles.Guest},
                new MenuItem
                {
                    MenuItemTitle = "Помощь",
                    ControllerName = "Support",
                    ActionName = "Index",
                    OrderNumber = 5,
                    MinimalAccessibleRole = Roles.Guest,
                    SubItems = new List<MenuItem>()
                    {
                        new MenuItem(){MenuItemTitle = "Cсылки", ControllerName = "Support", ActionName = "References",OrderNumber = 4, MinimalAccessibleRole = Roles.Guest, IdMenuItemCategory = 1},
                        new MenuItem(){MenuItemTitle = "Статьи", ControllerName = "Support", ActionName = "Articles", OrderNumber = 3, MinimalAccessibleRole = Roles.Guest, IdMenuItemCategory = 1},
                        new MenuItem(){MenuItemTitle = "О проекте", ControllerName = "Support", ActionName = "WhoWeAre", OrderNumber = 1, MinimalAccessibleRole = Roles.Guest, IdMenuItemCategory = 1},
                        new MenuItem(){MenuItemTitle = "Карта сайта", ControllerName = "Support", ActionName = "SiteMap", OrderNumber = 2, MinimalAccessibleRole = Roles.Guest,IdMenuItemCategory = 1},
                        new MenuItem()
                        {
                            MenuItemTitle = "Предложения по улучшению сайта", ControllerName = "Support", ActionName = "MemberOfferings", OrderNumber = 5, MinimalAccessibleRole = Roles.Guest,
                            IdMenuItemCategory = 1
                        },
                    }
                }
            };
            var maincategory = context.Set<MenuItemCategory>().First();
            maincategory.MenuItems.AddRange(items);
            context.SaveChanges();
        }

        private static void InitMenuItemCategories(PortalContext context)
        {
            var categories = new[]
            {
                new MenuItemCategory(){CategoryName = "Главное меню"},
                new MenuItemCategory(){CategoryName = "Меню администратора"},
                new MenuItemCategory(){CategoryName = "Меню модератора"},
                new MenuItemCategory(){CategoryName = "Меню участника контеста"}
            };
            context.Set<MenuItemCategory>().AddOrUpdate(x=>x.CategoryName,categories);
            context.SaveChanges();
        }


        private static void InitFeedbackTypes(PortalContext context)
        {
            var types = new[]
            {
                new FeedbackType {FeedbackTypeContent = "Предложение по улучшению функционала сайта"},
                new FeedbackType {FeedbackTypeContent = "Отчет об ошибке в работе сайта"},
                new FeedbackType {FeedbackTypeContent = "Заявление на перепросмотр отправленного решения"},
                new FeedbackType {FeedbackTypeContent = "Отзыв о работе судейства"}
            };
            context.Set<FeedbackType>().AddOrUpdate(x => x.FeedbackTypeContent, types);
        }
    }

    public static class Roles
    {
        public const string SuperAdministrator = "SuperAdministrator";
        public const string Administrator = "Administrator";
        public const string Moderator = "Moderator";
        public const string Judge = "Judge";
        public const string Competitor = "Competitor";
        public const string Member = "Member";
        public const string Guest = "Guest";
    }
}