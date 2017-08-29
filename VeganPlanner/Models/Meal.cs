using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeganPlanner.Models
{
    public class Meal
    {
        public int MealID { get; set; }
        public string NickName { get; set; }
        public string UserID { get; set; }
        public int CaloriesPerServing { get; set; }
        public int ProteinPerServing { get; set; }
        public string Notes { get; set; }

        public ICollection<MealComponent> MealComponents { get; set; }

    }
}
