using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestsPortal.Domain.Models
{
   public class ArchivedTask
    {
       public int TaskId { get; set; }
       public string TaskContent { get; set; }
       public int TaskComplexity { get; set; }
       public string TaskTitle { get; set; }

       public int? IdTaskCategory { get; set; }
       public TaskCategory TaskCategory { get; set; }
       public ICollection<ContestTask> ContestTasks { set; get; }
       public ICollection<CorrectSolution> CorrectSolutions { get; set; } 
    }
}
