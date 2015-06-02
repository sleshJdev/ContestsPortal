using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using ContestsPortal.Domain.Models;

namespace ContestsPortal.Domain.DataAccess
{
    public class UserProfileConfiguration : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileConfiguration()
        {
            ToTable("UserProfiles");
            HasKey(x => x.Id);
            Property(o => o.NickName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute {IsUnique = true}));
            Property(o => o.Email)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute {IsUnique = true}));
            Property(o => o.UserName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute {IsUnique = true}));

            //password hash is not required
            Property(o => o.PasswordHash).HasMaxLength(128);

            Property(x => x.Photo).IsOptional();
            Property(x => x.RegistrationDate).IsRequired().HasColumnType("datetime2");
            Property(o => o.FirstName).IsOptional().HasMaxLength(100);
            Property(o => o.LastName).IsOptional().HasMaxLength(100);
            Property(o => o.MiddleName).IsOptional().HasMaxLength(100);

            Property(o => o.ContestNotificationsEnabled).IsOptional();
            Property(o => o.HomePage).IsOptional().HasMaxLength(100);
            Property(o => o.AboutYourself).IsOptional().HasMaxLength(300);

            Property(o => o.Age)
                .IsOptional()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)
                .HasColumnType("Bigint");
            Property(o => o.FullName).IsOptional().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            HasMany(x => x.Competitors)
                .WithRequired(x => x.UserProfile)
                .HasForeignKey(x => x.IdProfile)
                .WillCascadeOnDelete(true);
            HasRequired(o => o.Country).WithMany(k => k.UserProfiles).WillCascadeOnDelete(false);
            HasOptional(o => o.UserActivity).WithRequired(x => x.UserProfile).WillCascadeOnDelete(true);
            HasMany(x => x.ProfileSettings)
                .WithRequired(x => x.UserProfile)
                .HasForeignKey(x => x.IdUserProfile)
                .WillCascadeOnDelete(true);
        }
    }


    public class CountryConfiguration : EntityTypeConfiguration<Country>
    {
        public CountryConfiguration()
        {
            ToTable("Countries", "handbooks");
            HasKey(x => x.CountryId);
            Property(x => x.CountryId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CountryName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute {IsUnique = true}));
        }
    }

    public class UserActivityConfiguration : EntityTypeConfiguration<UserActivity>
    {
        public UserActivityConfiguration()
        {
            ToTable("UserActivities");
            HasKey(x => x.IdUserProfile);
            HasRequired(x => x.UserProfile).WithOptional(x => x.UserActivity);
            Property(x => x.LastActivity).HasColumnType("datetime2");
        }
    }

    public class ProfileSettingConfiguration : EntityTypeConfiguration<ProfileSetting>
    {
        public ProfileSettingConfiguration()
        {
            ToTable("ProfileSettings");
            HasKey(x => x.SettingId);
        }
    }


    public class PostConfiguration : EntityTypeConfiguration<Post>
    {
        public PostConfiguration()
        {
            Property(x => x.PostTitle).IsRequired();
            Property(x => x.PostContent).IsRequired();
            Property(x => x.PublicationDate).IsRequired().HasColumnType("datetime2");
        }
    }


    public class CompetitorConfiguration : EntityTypeConfiguration<Competitor>
    {
        public CompetitorConfiguration()
        {
            HasKey(x => x.CompetitorId);
            Property(x => x.CompetitorId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.TotalGrade).IsOptional();
            Property(x => x.Ranking).IsOptional();
            HasRequired(x => x.UserProfile)
                .WithMany(x => x.Competitors)
                .HasForeignKey(x => x.IdProfile)
                .WillCascadeOnDelete(true);
            HasRequired(x => x.Contest)
                .WithMany(x => x.Competitors)
                .HasForeignKey(x => x.IdContest)
                .WillCascadeOnDelete(true);
            HasMany(x => x.Solutions)
                .WithRequired(x => x.Competitor)
                .HasForeignKey(x => x.IdCompetitor)
                .WillCascadeOnDelete(true);
        }
    }


    public class ContestConfiguration : EntityTypeConfiguration<Contest>
    {
        public ContestConfiguration()
        {
            HasKey(x => x.ContestId);
            Property(x => x.ContestBeginning).HasColumnType("datetime2");
            Property(x => x.ContestTitle)
                .IsRequired()
                .HasMaxLength(300)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute {IsUnique = true}));
            Property(x => x.ContestComment).IsOptional().HasMaxLength(300);
            HasMany(x => x.Competitors)
                .WithRequired(x => x.Contest)
                .HasForeignKey(x => x.IdContest)
                .WillCascadeOnDelete(true);
            HasMany(x => x.Tasks)
                .WithRequired(x => x.Contest)
                .HasForeignKey(x => x.IdContest)
                .WillCascadeOnDelete(false);
            HasRequired(x=>x.ContestPriority).WithMany(x=>x.RelatedContests).HasForeignKey(x=>x.IdContestPriority).WillCascadeOnDelete(false);
        }
    }


    public class ContestTaskConfiguration : EntityTypeConfiguration<ContestTask>
    {
        public ContestTaskConfiguration()
        {
            HasKey(x => x.TaskId);
            Property(x => x.TaskId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.TaskComplexity).IsRequired();
            Property(x => x.TaskComment).HasMaxLength(300);
            Property(x => x.TaskTitle).HasMaxLength(200).IsRequired();
            Property(x => x.TaskContent).IsRequired();
        }
    }

    public class TaskSolutionConfiguration : EntityTypeConfiguration<TaskSolution>
    {
        public TaskSolutionConfiguration()
        {
            HasKey(x => new {x.IdCompetitor, x.IdTask, x.SolutionId});
            Property(x => x.SolutionId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnOrder(1);
            Property(x => x.IdCompetitor).HasColumnOrder(2);
            Property(x => x.IdTask).HasColumnOrder(3);
            HasRequired(x => x.Competitor)
                .WithMany(x => x.Solutions)
                .HasForeignKey(x => x.IdCompetitor)
                .WillCascadeOnDelete(true);
            HasRequired(x => x.RelatedTask)
                .WithMany(x => x.Solutions)
                .HasForeignKey(x => x.IdTask)
                .WillCascadeOnDelete(false);
        }
    }


    public class ProgrammingLanguageConfiguration : EntityTypeConfiguration<ProgrammingLanguage>
    {
        public ProgrammingLanguageConfiguration()
        {
            ToTable("Languages", "handbooks");
            HasKey(x => x.LanguageId);
            Property(x => x.LanguageName).IsRequired().HasMaxLength(200)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute {IsUnique = true}));
            HasMany(x => x.Tasks).WithMany(x => x.Languages).Map(x =>
            {
                x.ToTable("LanguageTasks");
                x.MapLeftKey("IdLanguage");
                x.MapRightKey("IdTask");
            });
        }
    }

    public class TaskCategoryConfiguration : EntityTypeConfiguration<TaskCategory>
    {
        public TaskCategoryConfiguration()
        {
            ToTable("TaskCategories", "handbooks");
            HasKey(x => x.CategoryId);
            Property(x => x.CategoryName)
                .IsRequired()
                .HasMaxLength(300)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute {IsUnique = true}));
            HasMany(x => x.RelatedTasks)
                .WithOptional(x => x.TaskCategory)
                .HasForeignKey(x => x.IdTaskCategory)
                .WillCascadeOnDelete(false);
        }
    }


    public class ArchivedTaskConfiguration : EntityTypeConfiguration<ArchivedTask>
    {
        public ArchivedTaskConfiguration()
        {
            HasKey(x => x.TaskId);
            Property(x => x.TaskTitle).IsRequired().HasMaxLength(200);
            HasMany(x => x.ContestTasks)
                .WithOptional(x => x.ArchivedTask)
                .HasForeignKey(x => x.IdArchivedTask)
                .WillCascadeOnDelete(false);
            Property(x => x.TaskContent).IsRequired();
            Property(x => x.TaskComplexity).IsRequired();
        }
    }


    public class MenuItemCategoryConfiguration : EntityTypeConfiguration<MenuItemCategory>
    {
        public MenuItemCategoryConfiguration()
        {
            ToTable("MenuItemCategories", "handbooks");
            HasKey(x => x.CategoryId);
            Property(x => x.CategoryName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute {IsUnique = true}));
            HasMany(x => x.MenuItems)
                .WithOptional(x => x.MenuItemCategory)
                .HasForeignKey(x => x.IdMenuItemCategory)
                .WillCascadeOnDelete(false);
        }
    }

    public class MenuItemConfiguration : EntityTypeConfiguration<MenuItem>
    {
        public MenuItemConfiguration()
        {
            HasKey(x => x.MenuItemId);
            Property(x => x.MenuItemTitle)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute {IsUnique = true}));
            Property(x => x.ActionName).IsRequired().HasMaxLength(300);
            Property(x => x.ControllerName).IsRequired().HasMaxLength(300);
            Property(x => x.HoverDescription).IsOptional().HasMaxLength(300);
            HasMany(x => x.SubItems).WithOptional(x => x.ParentItem).HasForeignKey(x => x.IdParentMenuItem);
        }
    }


    public class FeedbackTypeConfiguration : EntityTypeConfiguration<FeedbackType>
    {
        public FeedbackTypeConfiguration()
        {
            ToTable("FeedbackTypes", "handbooks");
            HasKey(x => x.FeedbackTypeId);
            Property(x => x.FeedbackTypeContent)
                .IsRequired()
                .HasMaxLength(300)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute {IsUnique = true}));
        }
    }

    public class FeedbackConfiguration : EntityTypeConfiguration<Feedback>
    {
        public FeedbackConfiguration()
        {
            HasKey(x => x.FeedbackId);
            Property(x => x.PublicationDate).IsRequired();
            Property(x => x.ReferredEmail).IsRequired();
            Property(x => x.FeedbackContent).HasMaxLength(500).IsRequired();
            HasOptional(x => x.UserProfile).WithMany(x => x.Feedbacks).HasForeignKey(x => x.IdUserProfile);
            HasRequired(x => x.FeedbackType).WithMany(x => x.Feedbacks).HasForeignKey(x => x.IdFeedbackType);
        }
    }


    public class CorrectSolutionConfiguration : EntityTypeConfiguration<CorrectSolution>
    {
        public CorrectSolutionConfiguration()
        {
            HasKey(x => x.SolutionId);
            HasRequired(x => x.ArchivedTask)
                .WithMany(x => x.CorrectSolutions)
                .HasForeignKey(x => x.IdArchivedTask)
                .WillCascadeOnDelete(true);
            Property(x => x.SolutionContent).IsRequired();
            Property(x => x.SolutionComment).IsOptional().HasMaxLength(200);
        }
    }

    public class ContestStateConfiguration : EntityTypeConfiguration<ContestState>
    {
        public ContestStateConfiguration()
        {
            ToTable("ContestStates", "handbooks");
            HasKey(x => x.ContestStateId);
            Property(x => x.ContestStateComment).IsOptional().HasMaxLength(500);
            Property(x => x.ContestStateName).IsRequired().HasMaxLength(200)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute {IsUnique = true}));
            HasMany(x => x.Contests)
                .WithRequired(x => x.ContestState)
                .HasForeignKey(x => x.IdContestState)
                .WillCascadeOnDelete(false);
        }
    }

    public class ContestPriorityConfiguration : EntityTypeConfiguration<ContestPriority>
    {
        public ContestPriorityConfiguration()
        {
            ToTable("ContestPriorities", "handbooks");
            HasKey(x => x.ContestPriorityId);
            Property(x => x.ContestPriorityName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute {IsUnique = true}));
           /* HasMany(x => x.RelatedContests)
                .WithRequired(x => x.ContestPriority)
                .HasForeignKey(x => x.IdContestPriority)
                .WillCascadeOnDelete(false);*/
        }
    }
}