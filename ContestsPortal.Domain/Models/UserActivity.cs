using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestsPortal.Domain.Models
{
    public class UserActivity
    {
        public int IdUserProfile { get; set; }
        public DateTime? LastActivity { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
