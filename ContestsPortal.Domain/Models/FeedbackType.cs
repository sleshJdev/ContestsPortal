using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestsPortal.Domain.Models
{
   public class FeedbackType
    {
       public FeedbackType()
       {
           Feedbacks = new List<Feedback>();
       }

       public int FeedbackTypeId { get; set; }
       public string FeedbackTypeContent { get; set; }
       public virtual List<Feedback>  Feedbacks { get; set; }
    }
}
