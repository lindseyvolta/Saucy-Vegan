using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeganPlanner.Models
{
    public class Ingredient
    {
        public int IngredientID { get; set; }
		public int ItemID { get; set; }
		public decimal Quantity { get; set; }
		public string Units { get; set; }

    }
}
