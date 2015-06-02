using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ContestsPortal.Domain.DataAccess.Providers.Interfaces;
using ContestsPortal.Domain.Models;
using Microsoft.AspNet.Identity;

namespace ContestsPortal.Domain.DataAccess.Providers
{
    public class ContestProvider : IContestsProvider
    {
        #region Fields

        private readonly Func<PortalContext> _getContext;

        #endregion

        #region Constructors

        public ContestProvider()
        {
        }

        public ContestProvider(Func<PortalContext> context)
        {
            if (context == null) throw new ArgumentNullException("context");
            _getContext = context;
        }

        #endregion
        
        public Task<IList<Contest>> GetContestsByStateAsync(string contestState)
        {
            return Task<IList<Contest>>.Factory.StartNew(() =>
            {
                using (PortalContext context = _getContext())
                {
                    int contestStateId =
                        context.Set<ContestState>().Single(x => x.ContestStateName.Equals(contestState)).ContestStateId;
                    return context.Contests.Where(x => x.IdContestState == contestStateId).ToList();
                }
            });
        }

        public Task<IList<Contest>> GetContestsByDateAsync(DateTime? begin, DateTime? end)
        {
            return Task<IList<Contest>>.Factory.StartNew(() =>
            {
                if (begin == null && end == null) return new List<Contest>();
                using (PortalContext context = _getContext())
                {
                    if (begin == null)
                    {
                        DateTime endDate = end.Value.AddDays(1).Date;
                        return context.Contests.Where(x => x.ContestBeginning < endDate).ToList();
                    }
                    if (end == null)
                    {
                        DateTime beginDate = begin.Value.Date;
                        return context.Contests.Where(x => x.ContestBeginning > beginDate).ToList();
                    }
                    DateTime begindate = begin.Value.Date;
                    DateTime enddate = end.Value.AddDays(1).Date;
                    return
                        context.Contests.Where(x => x.ContestBeginning >= begindate && x.ContestEnd <= enddate).ToList();
                }
            });
        }

        public Task<IdentityResult> SetStateForContestAsync(int contestId, string contestState)
        {
            return Task<IdentityResult>.Factory.StartNew(() =>
            {
                using (PortalContext context = _getContext())
                {
                    int stateobjId =
                        context.Set<ContestState>().Single(x => x.ContestStateName.Equals(contestState)).ContestStateId;
                    Contest contest = context.Contests.Find(contestId);
                    if (contest == null) return IdentityResult.Failed(new[] {"Контест отсутствует в БД"});
                    contest.IdContestState = stateobjId;
                    context.SaveChanges();
                    return IdentityResult.Success;
                }
            });
        }

        public Task<IdentityResult> CreateContestAsync(Contest contest)
        {
            return Task<IdentityResult>.Factory.StartNew(() =>
            {
                var state = new ContestState {ContestStateId = 1, ContestStateName = ContestStates.Awaiting};
                var listLangs = new List<ProgrammingLanguage>();

                foreach (ContestTask contestTask in contest.Tasks)
                    foreach (ProgrammingLanguage language in contestTask.Languages)
                        if (listLangs.Find(x => x.LanguageId == language.LanguageId) == null)
                        {
                            listLangs.Add(language);
                        }

                using (PortalContext context = _getContext())
                {
                    context.Set<ContestState>().Attach(state);
                    contest.ContestState = state;

                    foreach (ContestTask contestTask in contest.Tasks)
                    {
                        contestTask.Contest = contest;
                        for (int index = 0; index < contestTask.Languages.Count; index++)
                        {
                            contestTask.Languages[index] =
                                listLangs.Find(x => x.LanguageId.Equals(contestTask.Languages[index].LanguageId));
                            context.Entry(contestTask.Languages[index]).State = EntityState.Unchanged;
                        }
                    }
                    context.Contests.Add(contest);
                    try
                    {
                        context.SaveChanges();
                        return IdentityResult.Success;
                    }
                    catch (Exception e)
                    {
                        return IdentityResult.Failed(new[] {e.ToString()});
                    }
                }
            });
        }

        public async Task<IdentityResult> DeleteContestAsync(int contestId)
        {
            using (PortalContext context = _getContext())
            {
                Contest contest = await context.Contests.FindAsync(contestId);
                if (contest == null)
                    return IdentityResult.Failed(new[]
                    {
                        string.Format("Контест с id={0} отсутствует в БД", contestId)
                    });

                try
                {
                    context.Contests.Remove(contest);
                    await context.SaveChangesAsync();
                    return IdentityResult.Success;
                }
                catch (Exception e)
                {
                    return IdentityResult.Failed(e.Message);
                }
            }
        }

        public Task<IList<Contest>> GetContestsAsync()
        {
            return Task<IList<Contest>>.Factory.StartNew(() =>
            {
                using (PortalContext context = _getContext())
                {                     
                    return context.Contests.ToList();
                }
            });
        }

        public Task<Contest> GetContest(int contestId)
        {
            return Task<Contest>.Factory.StartNew(() =>
            {
                using (PortalContext context = _getContext())
                {
                    Contest contest = context.Contests.Where(x => x.ContestId.Equals(contestId)).Include("Tasks").Include("Competitors").FirstOrDefault();
                                       
                    return contest;
                }
            });
        }
    }
}