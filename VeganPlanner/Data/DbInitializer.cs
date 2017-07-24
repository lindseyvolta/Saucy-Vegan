using System;
using System.Linq;
using VeganPlanner.Models;

namespace VeganPlanner.Data
{
    public class DbInitializer
    {
        public static void Initialize(VeganPlannerContext context)
        {
            context.Database.EnsureCreated();

        }
    }
}
