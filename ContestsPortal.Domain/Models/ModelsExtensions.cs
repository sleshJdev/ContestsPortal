using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ContestsPortal.Domain.Models
{
   public partial class UserProfile: IDisposable
    {
       public void Dispose()
       {
           
       }

       public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<UserProfile, int> manager)
       {
           var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
           // only 5 default claims are stored.
           return userIdentity;
       }

    }
}
