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

        public ICollection<Ingredient> Ingredients { get; set; }
        public ICollection<Step> Instructions { get; set; }
    }
}
