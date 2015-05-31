using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestsPortal.Domain.Models
{
   public class ContestPriority
    {
       public int ContestPriorityId { get; set; }
       public string ContestPriorityName { get; set; }
       public virtual List<Contest> RelatedContests { get; set; } 

       public ContestPriority()
       {
           RelatedContests = new List<Contest>();
       }
    }
}
