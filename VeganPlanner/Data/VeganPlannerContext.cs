using Microsoft.EntityFrameworkCore;

namespace VeganPlanner.Models
{
    public class VeganPlannerContext : DbContext
    {
        public VeganPlannerContext (DbContextOptions<VeganPlannerContext> options)
            : base(options)
        {
        }

        public DbSet<VeganPlanner.Models.Item> Item { get; set; }
        public DbSet<VeganPlanner.Models.Ingredient> Ingredient { get; set; }
        public DbSet<VeganPlanner.Models.Step> Step { get; set; }
    }
}
