﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrdersApi.Models;

namespace OrdersApi.Persistence
{
    public class OrdersContext : DbContext
    {
        public OrdersContext(DbContextOptions<OrdersContext> options) : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //TODO: Fix bug when convert from Enum to String and recieve in the MVC Project
            //var converter = new EnumToStringConverter<Status>();
           
            //builder.Entity<Order>()
            //    .Property(p => p.Status)
            //    .HasConversion(converter);                    
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}