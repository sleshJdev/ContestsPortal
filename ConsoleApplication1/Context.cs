using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
   public class MyContext:DbContext
   {
       public DbSet<Product> Products;
       public DbSet<Vendor> Vendors { get; set; }

       public MyContext()
       {
           Products = Set<Product>();
           Vendors = Set<Vendor>();
       }

       protected override void OnModelCreating(DbModelBuilder modelBuilder)
       {   
           modelBuilder.Entity<Product>().HasMany(x => x.Vendors).WithMany(x => x.Products).Map(x =>
           {
               x.ToTable("VendorProducts");
               x.MapLeftKey("IdProduct");
               x.MapRightKey("IdVendor");
           });
       }
   }
}
