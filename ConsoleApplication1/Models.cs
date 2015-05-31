using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
   public  class Product
    {
       public int ProductId { get; set; }
       public string ProductName { get; set; }
       public virtual ICollection<Vendor> Vendors  { get; set; }

       public Product()
       {
           Vendors = new List<Vendor>();
       }
    }



    public class Vendor
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        public Vendor()
        {
            Products = new List<Product>();
        }
    }

   
}
