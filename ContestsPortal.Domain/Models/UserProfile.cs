using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ContestsPortal.Domain.DataAccess;
using Microsoft.AspNet.Identity.EntityFramework;


namespace ContestsPortal.Domain.Models
{
    public partial  class UserProfile:IdentityUser<int,CustomIdentityUserLogin,CustomIdentityUserRole,CustomIdentityUserClaim>
    {

        #region Constructors

        public UserProfile()
        {
            ProfileSettings = new List<ProfileSetting>();
            Feedbacks = new List<Feedback>();
            Competitors = new List<Competitor>();
        }

        #endregion


        #region Properties

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public DateTime? BirthDate { get; set; }

        public long? Age {
            get
            {
                if (BirthDate == null)
                    return null;
                return (DateTime.Now - BirthDate).Value.Ticks;
            }
            set { }
        }

        public byte[] Photo { get; set; }

        public string FullName { get; set; }

        public string NickName { get; set; }

        public int IdCountry { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool ContestNotificationsEnabled { get; set; }

        public string HomePage { get; set; }

        public string AboutYourself { get; set; }

        [ForeignKey("IdCountry")]
        public Country Country { get; set; }

        public UserActivity UserActivity { get; set; }

        public virtual List<Competitor> Competitors { get; set; }

        public virtual List<ProfileSetting> ProfileSettings { get; set; }

        public virtual List<Feedback> Feedbacks { get; set; }
        
        #endregion

    }
}
