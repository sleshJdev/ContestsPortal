using System;

namespace ContestsPortal.Domain.Models
{
   public class Feedback
    {
       public int FeedbackId { get; set; }
       public string FeedbackContent { get; set; }
       public DateTime PublicationDate { get; set; }
       public string ReferredEmail { get; set; }

       public int IdFeedbackType { get; set; }
       public int? IdUserProfile { get; set; }

       public virtual UserProfile UserProfile { get; set; }
       public virtual FeedbackType  FeedbackType { get; set; }
    }
}
