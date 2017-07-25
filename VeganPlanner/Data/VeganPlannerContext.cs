using Microsoft.EntityFrameworkCore;
using VeganPlanner.Models;

namespace VeganPlanner.Models
{
    public class VeganPlannerContext : DbContext
    {
        public VeganPlannerContext(DbContextOptions<VeganPlannerContext> options)
            : base(options)
        {
        }

        public DbSet<VeganPlanner.Models.Item> Item { get; set; }
        public DbSet<VeganPlanner.Models.Ingredient> Ingredient { get; set; }
        public DbSet<VeganPlanner.Models.Step> Step { get; set; }
        public DbSet<VeganPlanner.Models.Recipe> Recipe { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().ToTable("Item");
            modelBuilder.Entity<Ingredient>().ToTable("Ingredient");
            modelBuilder.Entity<Step>().ToTable("Step");
            modelBuilder.Entity<Recipe>().ToTable("Recipe");
        }
    }
}
