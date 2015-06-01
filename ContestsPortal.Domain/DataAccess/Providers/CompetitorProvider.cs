using ContestsPortal.Domain.DataAccess.Providers.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestsPortal.Domain.DataAccess.Providers
{
    public class CompetitorProvider : ICompetitorProvider
    {
         #region Fields

        private readonly Func<PortalContext> _getContext;

        #endregion

        #region Constructors

        public CompetitorProvider()
        {
        }

        public CompetitorProvider(Func<PortalContext> context)
        {
            if (context == null) throw new ArgumentNullException("context");
            _getContext = context;
        }

        #endregion

        public Task<Microsoft.AspNet.Identity.IdentityResult> AddCompretitor(Models.Competitor competitor)
        {
            return Task<IdentityResult>.Factory.StartNew(() =>
            {
                using (PortalContext context = _getContext())
                {
                    context.Competitors.Add(competitor);
                    try
                    {
                        context.SaveChanges();
                        return IdentityResult.Success;
                    }
                    catch (Exception e)
                    {
                        return IdentityResult.Failed(new[] { e.ToString() });
                    }
                }
            });
        }
    }
}
