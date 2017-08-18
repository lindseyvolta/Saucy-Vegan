using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeganPlanner.Models
{
    public class Ingredient
    {
        public int IngredientID { get; set; }
        public Item item { get; set; }
		public int ItemID { get; set; }
        public int RecipeID { get; set; }
		public decimal Quantity { get; set; }
		public string Units { get; set; }

        public Ingredient Clone()
        {
            Ingredient new_ingredient = new Ingredient()
            {
                IngredientID = IngredientID,
                item = item,
                ItemID = ItemID,
                RecipeID = RecipeID,
                Quantity = Quantity,
                Units = Units
            };
            return new_ingredient;
        }
    }
}
