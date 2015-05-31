using System.Collections.Generic;

namespace ContestsPortal.Domain.Models
{
   public class MenuItem
    {
       public int MenuItemId { get; set; }
       public string MenuItemTitle { get; set; }
       public string HoverDescription { get; set; }
       public string ControllerName { get; set; }
       public string ActionName { get; set; }
       public int? IdParentMenuItem { get; set; }
       public int OrderNumber { get; set; }
       public string MinimalAccessibleRole  { get; set; }
       
       public virtual MenuItem ParentItem { get; set; }
       public virtual List<MenuItem> SubItems { get; set; }

       public int? IdMenuItemCategory { get; set; }
       public MenuItemCategory MenuItemCategory { get; set; }

       public MenuItem()
       {
           SubItems = new List<MenuItem>();
       }
    }
}
