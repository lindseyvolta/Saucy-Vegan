using System.ComponentModel.DataAnnotations;

namespace VeganPlanner.Models
{
    public class Item
    {
        public int ItemID { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsRecipe { get; set; }
        public decimal ServingSize { get; set; }
        public string ServingUnits { get; set; }
        public string Category { get; set; }
        public string UserID { get; set; }
        public int CaloriesPerServing { get; set; }
        public int ProteinPerServing { get; set; }
        [Display(Name = "Pantry Item")]
        public bool IsPantryItem { get; set; }
        [Display(Name = "GF")]
        public bool IsGF { get; set; }

        [DisplayFormat(NullDisplayText = "Not a Recipe")]
        public int? RecipeID { get; set; }
        public Recipe recipe { get; set; }
    }
}
