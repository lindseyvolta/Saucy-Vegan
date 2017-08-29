using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeganPlanner.Models
{
    public class MealComponent
    {
        public int MealComponentID { get; set; }
        public int MealID { get; set; }
        public int FoodItemID { get; set; }
        public Item FoodItem { get; set; }
    }
}
