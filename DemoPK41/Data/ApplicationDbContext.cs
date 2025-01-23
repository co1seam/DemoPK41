using DemoPK41.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoPK41.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Recipe> Recipes => Set<Recipe>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=./recipes.db");

        }
    }
}
