using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace VeganPlanner.Helpers
{
    public class Units
    {
        
        private List<string> fUnitList = new List<string>() { "lb", "Cup", "tsp", "Tbsp", "oz" };
        public ReadOnlyCollection<string> UnitList
        {
            get { return fUnitList.AsReadOnly(); }
        }

        // In this class we can define conversions, and methods to merge quantities of different units 
        // and to roll up units to largest possible unit

    }


}
