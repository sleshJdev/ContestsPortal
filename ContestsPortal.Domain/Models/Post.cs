using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestsPortal.Domain.Models
{
   public class Post
    {
       public int PostId { get; set; }
       public string PostContent { get; set; }
       public DateTime PublicationDate { get; set; }
    }
}
