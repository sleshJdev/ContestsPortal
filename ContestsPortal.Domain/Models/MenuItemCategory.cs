using System.Collections.Generic;

namespace ContestsPortal.Domain.Models
{
   public class MenuItemCategory
    {
       public int CategoryId { get; set; }
       public string CategoryName { get; set; }

       public virtual List<MenuItem> MenuItems { get; set; }

       public MenuItemCategory()
       {
           MenuItems = new List<MenuItem>();
       }
    }
}
