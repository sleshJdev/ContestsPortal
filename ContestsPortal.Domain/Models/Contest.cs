using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestsPortal.Domain.Models
{
    public class Contest
    {
        public int ContestId { get; set; }
        public string ContestTitle { get; set; }
        public string ContestComment { get; set; }
        public DateTime? ContestBeginning { get; set; }
        public int TasksCount { get; set; }
        public DateTime? ContestEnd { get; set; }
        public int CompetitorsCount  { get; set; }
        public int IdContestPriority { get; set; }
        public int IdContestState { get; set; }
        public ContestState ContestState { get; set; }
        public virtual List<Competitor> Competitors { get; set; }
        public virtual List<ContestTask> Tasks { get; set; }
        public virtual ContestPriority ContestPriority { get; set; }
        
        public Contest()
        {
            Competitors = new List<Competitor>();
            Tasks = new List<ContestTask>();
        }
    }
}
