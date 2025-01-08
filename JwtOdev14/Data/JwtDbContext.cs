using Microsoft.EntityFrameworkCore;
using JwtOdev14.Data;

namespace JwtOdev14.Data
{
    public class JwtDbContext : DbContext
    {
        public JwtDbContext(DbContextOptions<JwtDbContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "MartinEden@gmail.com",
                    Password = "ShameOfTheSun",
                },
                new User
                {
                    Id = 2,
                    Email = "MartiEden2@gmail.com",
                    Password = "Ephemera",
                });
        }
    }
}
