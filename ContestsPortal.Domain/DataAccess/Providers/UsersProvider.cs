using ContestsPortal.Domain.DataAccess.Providers.Interfaces;
using ContestsPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestsPortal.Domain.DataAccess.Providers
{
    public class UsersProvider : IUsersProvider
    {
        #region Fields

        private readonly Func<PortalContext> _getContext;

        #endregion

        #region Constructors

        public UsersProvider()
        {
        }

        public UsersProvider(Func<PortalContext> context)
        {
            if (context == null) throw new ArgumentNullException("context");
            _getContext = context;
        }

        #endregion
        
        public Task<IList<UserProfile>> GetAllUsers()
        {
            return Task<IList<UserProfile>>.Factory.StartNew(() =>
            {
                using (PortalContext context = _getContext())
                {                     
                    return context.Users.ToList();
                }
            });
        }

        public Task<UserProfile> GetUser(int userId)
        {
            return Task<UserProfile>.Factory.StartNew(() =>
            {
                using (PortalContext context = _getContext())
                {
                    return context.Users.Where(x => x.Id == userId).FirstOrDefault();
                }
            });
        }        
    }
}
