using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VeganPlanner.Models
{
    public class Recipe
    {
        public int RecipeID { get; set; }
        public string Notes { get; set; }
        [Required]
        public int Servings { get; set; }

        ICollection<Ingredient> Ingredients { get; set; }
        ICollection<Step> Instructions { get; set; }
    }
}
