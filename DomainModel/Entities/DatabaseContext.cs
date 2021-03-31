using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.Entities
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<UserTicket> UserTickets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=.\\SQLEXPRESS; database=QRDatabase;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u=>u.UserName).IsUnique();
            modelBuilder.Entity<User>().Property(u => u.EmailConfirmed).HasDefaultValue(false);
            //data seed for testing
             modelBuilder.Entity<Role>().HasData(
                 new Role { Id = 1, RoleName = "Admin" },
                 new Role { Id = 2, RoleName = "User" });
            modelBuilder.Entity<User>().HasData(
                new User { Id= 1,UserName="Admin",Password="Admin",Email="YourMail@gmail.com",EmailConfirmed = true,FirstName="Admin",LastName="Admin",Phone="",RoleId=1,Birthday = new DateTime(2000,1,1)}
            );
            modelBuilder.Entity<Event>().HasData(
                new Event { Id = 1, EventName = "Test Event", Date = DateTimeOffset.UtcNow.AddDays(2), Description = "Event 1" });
            modelBuilder.Entity<Ticket>().HasData(
                new Ticket { Id = 1, EventId = 1, TicketName = "Test Event Ticket", Description = "Test Event Ticket", ExpiryDate = DateTimeOffset.UtcNow.AddDays(2), Quantity = 2 });
        }
    }
}
