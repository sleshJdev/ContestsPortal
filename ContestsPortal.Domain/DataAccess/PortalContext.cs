using System.Data.Entity;
using System.Diagnostics;
using ContestsPortal.Domain.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ContestsPortal.Domain.DataAccess
{
    public class PortalContext : IdentityDbContext<UserProfile, CustomIdentityRole, int, CustomIdentityUserLogin, CustomIdentityUserRole, CustomIdentityUserClaim>
    {

        #region  Constructors

        public PortalContext()
            : base("name=ContestsStore")
        {
            Posts = Set<Post>();
            Contests = Set<Contest>();
            ArchivedTasks = Set<ArchivedTask>();
            Countries = Set<Country>();
            Languages = Set<ProgrammingLanguage>();
            ContestTasks = Set<ContestTask>();
            Competitors = Set<Competitor>();
            Configuration.ProxyCreationEnabled = false;
        }

        public static PortalContext Create()
        {
            return new PortalContext();
        }

        static PortalContext()
        {

            Debug.IndentLevel = 5;
            Debug.Indent();
            Debug.WriteLine("PortalContext Type has been initialized");

        }


        #endregion

        #region Properties

        public DbSet<Post> Posts { get; set; }
        public DbSet<Contest> Contests { get; set; }
        public DbSet<ContestTask> ContestTasks { get; set; }
        public DbSet<ArchivedTask> ArchivedTasks { get; set; }
        public DbSet<ProgrammingLanguage> Languages { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Competitor> Competitors { get; set; } 

        #endregion


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ContestStateConfiguration());
            modelBuilder.Configurations.Add(new ContestPriorityConfiguration());
            modelBuilder.Configurations.Add(new ContestConfiguration());
            modelBuilder.Configurations.Add(new ContestTaskConfiguration());
            modelBuilder.Configurations.Add(new CompetitorConfiguration());
            modelBuilder.Configurations.Add(new TaskSolutionConfiguration());
            modelBuilder.Configurations.Add(new ProgrammingLanguageConfiguration());
            modelBuilder.Configurations.Add(new UserProfileConfiguration());
            modelBuilder.Configurations.Add(new CountryConfiguration());
            modelBuilder.Configurations.Add(new UserActivityConfiguration());
            modelBuilder.Configurations.Add(new ProfileSettingConfiguration());
            modelBuilder.Configurations.Add(new PostConfiguration());
            modelBuilder.Configurations.Add(new ArchivedTaskConfiguration());
            modelBuilder.Configurations.Add(new TaskCategoryConfiguration());
            modelBuilder.Configurations.Add(new MenuItemCategoryConfiguration());
            modelBuilder.Configurations.Add(new MenuItemConfiguration());
            modelBuilder.Configurations.Add(new FeedbackConfiguration());
            modelBuilder.Configurations.Add(new FeedbackTypeConfiguration());
            modelBuilder.Configurations.Add(new CorrectSolutionConfiguration());
            base.OnModelCreating(modelBuilder);
        }

    }



}
