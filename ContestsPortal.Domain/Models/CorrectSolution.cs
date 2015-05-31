using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestsPortal.Domain.Models
{
    public class CorrectSolution
    {
        public int  SolutionId { get; set; }
        public string SolutionContent { get; set; }
        public string SolutionComment { get; set; }

        public int IdArchivedTask { get; set; }
        public virtual ArchivedTask ArchivedTask { get; set; }
    }
}
