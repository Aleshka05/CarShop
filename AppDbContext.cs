using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System;
using System.Windows;
using CarShop.Model;
namespace CarShopApp
{
    public class CarShopContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarConfiguration> Configuration { get; set; }

        public DbSet<Review> Reviews { get; set; }
        public DbSet<Order> Orders { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
            optionsBuilder.UseSqlServer("Server=ALEKSEY;Database=CarShopDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Администратор" },
                new Role { Id = 2, Name = "Клиент" }
            );

            
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword("11111");
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = "Admin",
                    Email = "admin@carshop.com",
                    Phone = "+1234567890",
                    Login = "Admin",
                    Password = hashedPassword, 
                    RoleId = 1 
                }
           
           );
            modelBuilder.Entity<Car>()
       .HasOne(c => c.Configuration)
       .WithOne(cfg => cfg.Car)
       .HasForeignKey<CarConfiguration>(cfg => cfg.CarID);

            modelBuilder.Entity<CarConfiguration>()
                .HasKey(cfg => cfg.ConfigurationID);
        }

       
       
    }

   


}
