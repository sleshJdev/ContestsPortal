using ContestsPortal.Domain.DataAccess.Providers.Interfaces;
using ContestsPortal.Domain.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestsPortal.Domain.DataAccess.Providers
{
    public class ProgrammingLanguageProvider : IProgrammingLanguageProvider
    {
        #region Fields

        private readonly Func<PortalContext> _getContext;

        #endregion

        #region Constructors

        public ProgrammingLanguageProvider()
        {
        }

        public ProgrammingLanguageProvider(Func<PortalContext> context)
        {
            if (context == null) throw new ArgumentNullException("context");
            _getContext = context;
        }

        #endregion

        public async Task<IdentityResult> DeleteContestAsync(int languageId)
        {
            using (PortalContext context = _getContext())
            {
                ProgrammingLanguage contest = await context.Languages.FindAsync(languageId);
                if (contest == null)
                    return IdentityResult.Failed(new[]
                    {
                        string.Format("Язык с id={0} отсутствует в БД", languageId)
                    });

                try
                {
                    context.Languages.Remove(contest);
                    await context.SaveChangesAsync();
                    return IdentityResult.Success;
                }
                catch (Exception e)
                {
                    return IdentityResult.Failed(e.Message);
                }
            }
        }

        public Task<IdentityResult> AddProgrammingLanguageAsync(ProgrammingLanguage newLanguage)
        {
            return Task<IdentityResult>.Factory.StartNew(() =>
            {
                using (PortalContext context = _getContext())
                {
                    context.Languages.Add(newLanguage);
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

        public Task<IList<ProgrammingLanguage>> GetAllProgrammingLanguagesAsync()
        {
            return Task<IList<ProgrammingLanguage>>.Factory.StartNew(() =>
            {
                using (PortalContext context = _getContext())
                {                     
                    return context.Languages.ToList();
                }
            });
        }

        public Task<ProgrammingLanguage> GetProgrammingLanguageAsync(int languageId)
        {
            return Task<ProgrammingLanguage>.Factory.StartNew(() =>
            {
                using (PortalContext context = _getContext())
                {
                    return context.Languages.Where(x => x.LanguageId == languageId).FirstOrDefault();
                }
            });            
        }
    }
}
