using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace VeganPlanner.Models
{
    public class ItemCategoryViewModel
    {
        public List<Item> items;
        public SelectList categories;
        public string itemCategory { get; set; }
    }
}
