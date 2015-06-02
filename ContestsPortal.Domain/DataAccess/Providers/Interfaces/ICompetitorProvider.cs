using ContestsPortal.Domain.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestsPortal.Domain.DataAccess.Providers.Interfaces
{
    public interface ICompetitorProvider
    {
        Task<IList<Competitor>> GetAllCompretitorAsync();

        Task<IdentityResult> AddCompretitor(Competitor competitor);        
    }
}
