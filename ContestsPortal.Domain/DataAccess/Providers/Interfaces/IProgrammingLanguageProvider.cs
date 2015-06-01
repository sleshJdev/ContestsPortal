using ContestsPortal.Domain.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestsPortal.Domain.DataAccess.Providers.Interfaces
{
    public interface IProgrammingLanguageProvider
    {
        Task<IList<ProgrammingLanguage>> GetAllProgrammingLanguagesAsync();

        Task<ProgrammingLanguage> GetProgrammingLanguageAsync(int languageId);

        Task<IdentityResult> AddProgrammingLanguageAsync(ProgrammingLanguage newLanguage);

        Task<IdentityResult> DeleteContestAsync(int languageId);
    }
}
