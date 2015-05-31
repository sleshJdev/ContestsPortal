using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestsPortal.Domain.Models
{
   public class ContestState
    {
       public int ContestStateId { get; set; }
       public string ContestStateName { get; set; }
       public string ContestStateComment { get; set; }
       public List<Contest> Contests { get; set; }

       public ContestState()
       {
           Contests = new List<Contest>();
       }
    }


    public static class ContestStates
    {
        public const string Awaiting = "Ожидание";
        public const string Registration = "Регистрация участников";
        public const string Active = "Контест в данный момент проводится";
        public const string ResultsEvaluation = "Выполняется подведение итогов контеста";
        public const string Completed = "Контест завершен";
    }
}
