using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeganPlanner.Models
{
    public class Step
    {
        public int StepID { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }
}
