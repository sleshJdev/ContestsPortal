using System;
using System.Data.Entity;
using System.Linq;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            ManyToManyDemo();
            Console.ReadKey(true);
        }


        private static void ManyToManyDemo()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<MyContext>());
            using (var context = new MyContext())
            {
                var vendor = new Vendor(){VendorName = "A little startuper."};
                vendor.Products.Add(new Product(){ProductName = "Bubble Gum"});
                vendor.Products.Add(new Product(){ProductName = "Bread"});
                context.Vendors.Add(vendor);

                var product = new Product(){ProductName = "Milky Way"};
                product.Vendors.Add(new Vendor(){VendorName = "Bruce Willis"});
                product.Vendors.Add(new Vendor() { VendorName = "That's me." });
                product.Vendors.Add(vendor);
                context.Products.Add(product);
                context.SaveChanges();
            }

          
                using (var context= new MyContext())
                {
                    var vendor = context.Vendors.First();
                    var str = context.Database.Connection;
                    Console.WriteLine("Vendor: {0}", vendor.VendorName);
                    Console.WriteLine("Products:");
                    foreach (var vend in vendor.Products)
                    {
                        Console.WriteLine("ProductId: {0}",vend.ProductId);
                        Console.WriteLine("ProductName: {0}", vend.ProductName);
                        Console.WriteLine();
                    }

                    var product = context.Products.First(x => x.ProductName.Equals("Milky Way"));
                    Console.WriteLine("Product: {0}", product.ProductName);
                    Console.WriteLine("Vendors:");
                    foreach (var vend in product.Vendors)
                    {
                        Console.WriteLine("VendorId: {0}", vend.VendorId);
                        Console.WriteLine("VendorName: {0}", vend.VendorName);
                        Console.WriteLine();
                    }
                }
        }
    }
}
