﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;


namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        }
        public DbSet<Dish>Dishes{ get; set; }
        public DbSet<Category>Categories{ get; set; }
        public DbSet <Status> Statuses{ get; set; }
        public DbSet<DeliveryType> DeliveryTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        //CONFIGURATIONS OF RELATIONSHIPS
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.ToTable("Dish");
                entity.HasKey(e => e.DishId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Price).HasPrecision(18,2).IsRequired();
                entity.Property(e => e.CreateDate).IsRequired();
                entity.Property(e => e.UpdateDate).IsRequired();
                // Relationship with Category
                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Dishes)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
                // Relationship with OrderItems
                entity.HasMany(e => e.OrderItems)
                      .WithOne(oi => oi.Dish)
                      .HasForeignKey(oi => oi.DishId)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(25);
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.Order).IsRequired();

                // entradas
                entity.HasData(
                new Category { Id = 1, Name = "Entradas", Description = "Pequeñas porciones para abrir el apetito antes del plato principal.", Order = 1 },
                new Category { Id = 2, Name = "Ensaladas", Description = "Opciones frescas y livianas, ideales como acompañamiento o plato principal.", Order = 2 },
                new Category { Id = 3, Name = "Minutas", Description = "Platos rápidos y clásicos de bodegón: milanesas, tortillas, revueltos.", Order = 3 },
                new Category { Id = 4, Name = "Pastas", Description = "Variedad de pastas caseras y salsas tradicionales.", Order = 5 },
                new Category { Id = 5, Name = "Parrilla", Description = "Cortes de carne asados a la parrilla, servidos con guarniciones.", Order = 4 },
                new Category { Id = 6, Name = "Pizzas", Description = "Pizzas artesanales con masa casera y variedad de ingredientes.", Order = 7 },
                new Category { Id = 7, Name = "Sandwiches", Description = "Sandwiches y lomitos completos preparados al momento.", Order = 6 },
                new Category { Id = 8, Name = "Bebidas", Description = "Gaseosas, jugos, aguas y opciones sin alcohol.", Order = 8 },
                new Category { Id = 9, Name = "Cerveza Artesanal", Description = "Cervezas de producción artesanal, rubias, rojas y negras.", Order = 9 },
                new Category { Id = 10, Name = "Postres", Description = "Clásicos dulces caseros para cerrar la comida.", Order = 10 }
                );
            });


            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(25);
                entity.HasData(
                new Status { Id = 1, Name = "Pendiente" },
                new Status { Id = 2, Name = "En preparación" },
                new Status { Id = 3, Name = "Listo" },
                new Status { Id = 4, Name = "Entregado" },
                new Status { Id = 5, Name = "Cancelado" } 
                 );
            });


            modelBuilder.Entity<DeliveryType>(entity =>
            {
                entity.ToTable("DeliveryType");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasColumnName("nvarchar(25)");
                entity.HasData(
                new DeliveryType { Id = 1, Name = "Delivery" },
                new DeliveryType { Id = 2, Name = "Takeaway" },
                new DeliveryType { Id = 3, Name = "Dine in" }

                );
             });



            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");
                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.OrderId).ValueGeneratedOnAdd();
                entity.Property(e => e.DeliveryTo).HasMaxLength(255);
                entity.Property(e => e.Price).HasPrecision(18, 2).IsRequired();
                entity.Property(e => e.UpdateDate).IsRequired();
                entity.Property(e => e.CreateDate).IsRequired();

                // Relationship with DeliveryType
                entity.HasOne(e => e.DeliveryType)
                      .WithMany(d => d.Orders)
                      .HasForeignKey(e => e.DeliveryTypeId)
                      .OnDelete(DeleteBehavior.Restrict);
                // Relationship with Status
                entity.HasOne(e => e.OverallStatus)
                      .WithMany(s => s.Orders)
                      .HasForeignKey(e => e.StatusId)
                      .OnDelete(DeleteBehavior.Restrict);
                // Relationship with OrderItems
                entity.HasMany(e => e.OrderItems)
                      .WithOne(oi => oi.Order)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItem");
                entity.HasKey(e => e.OrderItemId);
                entity.Property(e => e.OrderItemId).ValueGeneratedOnAdd();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.CreateDate).IsRequired();

                // Relationship with Status
                entity.HasOne(e => e.Status)
                      .WithMany(s => s.OrderItems)
                      .HasForeignKey(e => e.StatusId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}
