using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestsPortal.Domain.Models
{
    
   public class ProfileSetting
    {

       #region Constructors
       
       #endregion

       #region Properties

       public int SettingId { get; set; }

       public int IdUserProfile { get; set; }

       public string SettingName { get; set; }

       public virtual UserProfile UserProfile { get; set; }

       #endregion

    }
}
