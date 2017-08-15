using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using VeganPlanner.Models;
using System;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VeganPlanner.Controllers
{
    public class ItemsController : Controller
    {
        private readonly VeganPlannerContext _context;

        public ItemsController(VeganPlannerContext context)
        {
            _context = context;    
        }

        // Requires using Microsoft.AspNetCore.Mvc.Rendering;
        public ActionResult Index()
        {
            return View("Index");
        }

        public async Task<IActionResult> GetItems(string searchString, string itemCategory)
        {
            // Use LINQ to get list of genres.
            IQueryable<string> categoryQuery = from m in _context.Item
                                            orderby m.Category
                                            select m.Category;

            var items = from m in _context.Item
                        .Include(c => c.recipe)
                             .ThenInclude(c => c.Ingredients)
                             .ThenInclude(c => c.item)
                         .Include(c => c.recipe)
                             .ThenInclude(c => c.Instructions)
                         .AsNoTracking()
                        select m;

            if (!String.IsNullOrEmpty(searchString))
                items = items.Where(s => s.Name.Contains(searchString));

            if (!String.IsNullOrEmpty(itemCategory))
                items = items.Where(x => x.Category == itemCategory);

            return Json(new { items = await items.ToListAsync() });
        }


        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
           .Include(c => c.recipe)
               .ThenInclude(c => c.Ingredients)
               .ThenInclude(c => c.item)
           .Include(c => c.recipe)
               .ThenInclude(c => c.Instructions)
           .AsNoTracking()
           .SingleOrDefaultAsync(m => m.ItemID == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        
        // GET: Items/Create
        public IActionResult Create()
        {
            PopulateItemsDropDownList();
            return View();
        }

        public ActionResult AddNewIngredient()
        {
            return PartialView("IngredientPartial", new Ingredient());
        }

        private void PopulateItemsDropDownList(object selectedItem = null)
        {
            var itemsQuery = from i in _context.Item
                             where i.IsRecipe == false
                             orderby i.Name
                             select i;

            ViewBag.itemList = new SelectList(itemsQuery.AsNoTracking(), "ItemID", "Name", selectedItem);
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemID,Name,IsRecipe,ServingSize,ServingUnits,Category,UserID,CaloriesPerServing,ProteinPerServing,IsPantryItem,IsGF,RecipeID,recipe")] Item item)
        {
            if (!item.IsRecipe)
            {
                item.RecipeID = null;
            }
            else
            {
                //add recipe to database and store corresponding recipe ID in item object
                _context.Recipe.Add(item.recipe);
                await _context.SaveChangesAsync();
                item.RecipeID = item.recipe.RecipeID;

                //adding ingredients to database
                foreach(var i in item.recipe.Ingredients)
                {
                    i.RecipeID = item.recipe.RecipeID;
                    _context.Ingredient.Add(i);
                }
                await _context.SaveChangesAsync();

                //adding instructions to database
                foreach (var i in item.recipe.Instructions)
                {
                    i.RecipeID = item.recipe.RecipeID;
                    _context.Step.Add(i);
                }
                await _context.SaveChangesAsync();

            }

            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: Items/Edit/5
        /*public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .Include(c => c.recipe)
                    .ThenInclude(c => c.Ingredients)
                    .ThenInclude(c => c.item)
                .Include(c => c.recipe)
                    .ThenInclude(c => c.Instructions)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ItemID == id);
            if (item == null)
            {
                return NotFound();
            }
            PopulateItemsDropDownList();
            return View(item);
        }*/

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        public string Edit(string itemJson)
        {
            Item item = Newtonsoft.Json.JsonConvert.DeserializeObject<Item>(itemJson);

            return "hello" + item.Name;
        }

        [HttpPost]
        public JsonResult Test(string json)
        {
            try
            {
                //dynamic item = JObject.Parse(json);
                Item item = Newtonsoft.Json.JsonConvert.DeserializeObject<Item>(json);

                _context.Entry(item).State = EntityState.Modified;
                _context.Entry(item.recipe).State = EntityState.Modified;
                if (item.IsRecipe)
                { 
                    foreach (Ingredient i in item.recipe.Ingredients)
                    {
                         _context.Entry(i).State = EntityState.Modified;
                    }
                }

                _context.SaveChanges();

                //_context.Item.Update
                string temp = "hello " + item.Name;
                if ((item.IsRecipe) && (item.recipe.Ingredients.Count > 0))
                {
                  foreach (Ingredient i in item.recipe.Ingredients)
                  {
                        temp = temp + " ingredient item =" + i.item.Name + " units =" + i.Units + " qty = " + i.Quantity.ToString();
                  }
                }
                //return "hello" + item.Name;
                return Json(temp);
            }
            catch (Exception e)
            {
                string temp = "An error occurred: " + e.Message;
                return Json(temp);
            }
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .Include(c => c.recipe)
                    .ThenInclude(c => c.Ingredients)
                .Include(c => c.recipe)
                    .ThenInclude(c => c.Instructions)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ItemID == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Item item = _context.Item
                .Include(m => m.recipe)
                .Where(m => m.ItemID == id)
                .Single();

            if (item.IsRecipe)
            {
                Recipe recipe = _context.Recipe
                        .Where(m => m.RecipeID == item.RecipeID)
                        .Single();
                _context.Recipe.Remove(recipe);
            }
            

            _context.Item.Remove(item);
           

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.ItemID == id);
        }
    }
}
