using System;
using System.Collections.Generic;

namespace ContestsPortal.Domain.Models
{
   public class ProgrammingLanguage
    {
       public int LanguageId { get; set; }
       
       public string LanguageName { get; set; }

       public virtual List<ContestTask> Tasks { get; set; }

       public ProgrammingLanguage(string langname)
       {
           if (string.IsNullOrEmpty(langname)) throw new ArgumentNullException("langname");
           LanguageName = langname;
       }

       public ProgrammingLanguage()
       {
           Tasks = new List<ContestTask>();
       }

    }
}
