using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ContestsPortal.Domain.Models
{
   public class TaskSolution
    {

       public int SolutionId  { get; set; }
       
       public int IdTask { get; set; }

       public int IdCompetitor { get; set; }

       public string SolutionContent { get; set; }

       public string SolutionComment { get; set; }

       public int TaskSolutionGrade { get; set; }

       // номер попытки
       public int SolutionOrderNumber { get; set; }

       public virtual Competitor  Competitor { get; set; }

       public virtual ContestTask RelatedTask { get; set; }

    }
}
