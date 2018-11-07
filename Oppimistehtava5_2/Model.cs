using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Oppimistehtava5_2
{
    public class ShopContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderRow> OrderRows { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=shop.db");
        }
    }

    public class Order
    {
        public int Id { get; set; }
        // public string Name { get; set; }
        // Otin tilauksen nimen pois.
        // Oliko tehtävänannossa tarkoitus, että tämä
        // nimikenttä viittaa tilaajaan?

        // SQLite-kannassa ei voinut suoraan tehdä alter tablea,
        // joten vahingon jälkeen tein copy-pastella projektin uudelleen.
        public DateTime Ordered { get; set; }
        public OrderStatus Status { get; set; }
        public IList<OrderRow> OrderRows { get; set; }
        public Customer Customer { get; set; }

        public override string ToString()
        {
            //var status = 
            string orderText = $"Tilaus {Id}: {Ordered} ({Status})";
            return orderText;
        }
    }

    public class OrderRow
    {
        public int Id { get; set; }
        public float Discount { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public IList<OrderRow> OrderRows { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public IList<Order> Orders { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public enum OrderStatus
    {
        Tilattu = 1,
        Maksettu = 2,
        Toimitettu = 3,
        Palautettu = 4
    }
}