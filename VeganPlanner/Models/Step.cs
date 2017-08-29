using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeganPlanner.Models
{
    public class Step
    {
        public int StepID { get; set; }
        public int RecipeID { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        public Step Clone()
        {
            Step new_ingredient = new Step()
            {
                StepID = StepID,
                RecipeID = RecipeID,
                Description = Description,
                Order = Order
            };
            return new_ingredient;
        }
    }
}
