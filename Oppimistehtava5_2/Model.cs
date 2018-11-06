﻿using System;
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
        public DateTime Ordered { get; set; }
        public OrderStatus Status { get; set; }
        public IList<OrderRow> OrderRows { get; set; }
        public Customer Customer { get; set; }
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
    }

    public enum OrderStatus
    {
        Orderd = 1,
        Paid = 2,
        Shipped = 3,
        Returned = 4
    }
}