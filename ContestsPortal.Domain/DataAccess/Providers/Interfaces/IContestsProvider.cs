using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContestsPortal.Domain.Models;
using Microsoft.AspNet.Identity;

namespace ContestsPortal.Domain.DataAccess.Providers.Interfaces
{
  public interface IContestsProvider
  {
      Task<Contest> GetContest(int contestId);
      Task<IList<Contest>> GetContestsAsync();
      Task<IList<Contest>> GetContestsByStateAsync(string contestState);
      Task<IList<Contest>> GetContestsByDateAsync(DateTime? begin, DateTime? end);
      Task<IdentityResult> SetStateForContestAsync(int contestId, string contestState);
      Task<IdentityResult> CreateContestAsync(Contest contest);
      Task<IdentityResult> DeleteContestAsync(int contestId);     
  }
}
