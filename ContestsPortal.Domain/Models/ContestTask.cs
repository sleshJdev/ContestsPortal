using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestsPortal.Domain.Models
{
    public class  ContestTask
    {
        public int TaskId { get; set; }

        public int? TaskComplexity { get; set; }
        /// <summary>
        /// Duration in seconds.
        /// </summary>
        public TimeSpan? TaskDuration { get; set; }

        public string TaskTitle { get; set; }

        public string TaskContent { get; set; }

        public string TaskComment { get; set; }

        public int TaskAward { get; set; }

        public int IdContest { get; set; }

        public int? IdArchivedTask { get; set; } 

        public virtual Contest Contest { get; set; }

        public virtual List<TaskSolution> Solutions { get; set; }

        public virtual List<ProgrammingLanguage> Languages  { get; set; }

        public virtual ArchivedTask ArchivedTask { get; set; }

        public ContestTask()
        {
            Languages = new List<ProgrammingLanguage>();
            Solutions = new List<TaskSolution>();
        }
    }
}
