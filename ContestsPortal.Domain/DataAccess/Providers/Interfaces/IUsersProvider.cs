using ContestsPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestsPortal.Domain.DataAccess.Providers.Interfaces
{
    public interface IUsersProvider
    {
        Task<IList<UserProfile>> GetAllUsers();
        Task<UserProfile> GetUser(int userId);
    }
}
