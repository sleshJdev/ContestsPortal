using System.Collections.Generic;

namespace ContestsPortal.Domain.Models
{
  public class Competitor
    {
      public int CompetitorId { get; set; }

      public int IdContest { get; set; }

      public int IdProfile { get; set; }

      // порядковый номер в рейтинге (место, которое заял участник).
      public int Ranking { get; set; }

      public int TotalGrade { get; set; }

      public UserProfile UserProfile { get; set; }
      
      public Contest Contest { get; set; }

      public virtual ICollection<TaskSolution> Solutions { get; set; }
    }
}
