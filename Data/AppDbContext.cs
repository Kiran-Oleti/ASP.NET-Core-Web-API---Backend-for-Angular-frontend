using Microsoft.EntityFrameworkCore;
using Registration.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Registration.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add any model configurations here using modelBuilder.Entity<T>()
        }

        public DbSet<User> Users { get; set; }
    }
}
