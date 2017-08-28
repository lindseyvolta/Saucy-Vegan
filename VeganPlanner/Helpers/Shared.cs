using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeganPlanner.Helpers
{
    public static class Shared
    {
        public static Units Units = new Helpers.Units();

        public static List<string> CategoryList = new List<string>() { "Protein", "Raw Protein", "Vegetable", "Sauce", "Milk/Creamer" };
    }
}
