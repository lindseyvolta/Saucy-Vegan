using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using VeganPlanner.Models;
using System;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

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
            var categoryQuery = from m in _context.Item
                                            orderby m.Category
                                            select m.Category;
            
            //ViewData["categories"] = new SelectList(categoryQuery);

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

        public async Task<IActionResult> GetItemsDropDown()
        {
            var itemsQuery = from i in _context.Item
                            .AsNoTracking()
                             where i.IsRecipe == false
                             orderby i.Name
                             select i;

            //System.IO.File.WriteAllText("c:\\temp\\myfile.txt", "count = "+ itemsQuery.ToList().Count.ToString());

            return Json(new { itemsQuery = await itemsQuery.ToListAsync() });

        }

        
        // GET: Items/Create
        public IActionResult Create()
        {
            //PopulateItemsDropDownList();
            return View();
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

 
        [HttpPost]
        public JsonResult Edit(string json)
        {
            string temp = "Start Edit"+Environment.NewLine;
            try
            {
                
                Item item = Newtonsoft.Json.JsonConvert.DeserializeObject<Item>(json);
                if ((item.IsRecipe) && (item.recipe.Ingredients.Count > 0))
                {
                    foreach (Ingredient i in item.recipe.Ingredients)
                    {
                        if (i.item == null)
                        {                            
                            i.item = _context.Item.Where(a => a.ItemID == i.ItemID).SingleOrDefault();                                
                        }
                        //temp = temp + " ingredient item =" + i.item.Name + " units =" + i.Units + " qty = " + i.Quantity.ToString()+" ingredient id = "+i.IngredientID+Environment.NewLine;
                    }

                } 

                var itemdb = _context.Item.Where(a => a.ItemID == item.ItemID)
                                .Include(c => c.recipe)
                                    .ThenInclude(c => c.Ingredients)
                                    .ThenInclude(c => c.item)
                                .Include(c => c.recipe)
                                    .ThenInclude(c => c.Instructions)
                                .SingleOrDefault();                


                // if NOT a recipe now - but was before edit, need to delete recipe, instructions, and ingredients
                if (itemdb.IsRecipe && !item.IsRecipe)
                {

                    foreach (var child in itemdb.recipe.Ingredients.ToList())
                    {
                        itemdb.recipe.Ingredients.Remove(child);
                        _context.Ingredient.Remove(child);                       
                    }

                    foreach (var child in itemdb.recipe.Instructions.ToList())
                    {
                        itemdb.recipe.Instructions.Remove(child);
                        _context.Step.Remove(child);                        
                    }                    
                    _context.Recipe.Remove(itemdb.recipe);
                    item.RecipeID = 0;   // be sure these are clear before setting values in itemdb, below;
                    item.recipe = null;
                }

                _context.Entry(itemdb).CurrentValues.SetValues(item);

                if (item.IsRecipe)
                {
                    _context.Entry(itemdb.recipe).State = EntityState.Modified;
                    foreach (Ingredient i in item.recipe.Ingredients)
                    {
                        var currIngredient = itemdb.recipe.Ingredients.FirstOrDefault(x => x.IngredientID == i.IngredientID);
                        if (currIngredient == null)
                        {
                            itemdb.recipe.Ingredients.Add(i);  // sample code used i.Clone(), but then ingredient id is not updated in item... delete doesn't work.
                            _context.SaveChanges();  // need to save to get new/unique ID otherwise can't add more than one also EF changes from 0 to -2147482647 at some point, and delete loop deletes it.                        
                        }
                        else
                            _context.Entry(currIngredient).CurrentValues.SetValues(i);
                    }

                    foreach (Ingredient i in itemdb.recipe.Ingredients)
                    {
                        temp = temp + "itemdb.ingredientid = " + i.IngredientID + Environment.NewLine;
                    }

                    foreach (Step s in item.recipe.Instructions)
                    {
                        var currInstruction = itemdb.recipe.Instructions.FirstOrDefault(x => x.StepID == s.StepID);
                        if (currInstruction == null)
                        {
                            itemdb.recipe.Instructions.Add(s);
                            _context.SaveChanges();                            
                        }
                        else
                            _context.Entry(currInstruction).CurrentValues.SetValues(s);
                    }

                    // Now delete all entries in itemdb.recipe.Ingredients but missing in item.recipe.Ingredients
                    // ToList should make copy of the collection because we can't modify collection iterated by foreach                    
                    foreach (var child in itemdb.recipe.Ingredients.ToList())
                    {
                        var detachedChild = item.recipe.Ingredients.FirstOrDefault(x => x.IngredientID == child.IngredientID);
                        if (detachedChild == null)
                        {
                            //temp = temp + "remove Ingredient " + child.IngredientID + Environment.NewLine;
                            itemdb.recipe.Ingredients.Remove(child);
                            _context.Ingredient.Remove(child);
                        }
                    }

                    // Now delete all entries in itemdb.recipe.Instructions but missing in item.recipe.Instructions
                    // ToList should make copy of the collection because we can't modify collection iterated by foreach
                    foreach (var child in itemdb.recipe.Instructions.ToList())
                    {
                        var detachedChild = item.recipe.Instructions.FirstOrDefault(x => x.StepID == child.StepID);
                        if (detachedChild == null)
                        {
                            //temp = temp + "remove Step " + child.StepID;
                            itemdb.recipe.Instructions.Remove(child);
                            _context.Step.Remove(child);
                        }
                    }

                }                

                //System.IO.File.WriteAllText("c:\\temp\\myfile.txt", temp);

                _context.SaveChanges();

                return Json(temp);
            }
            catch (Exception e)
            {
                temp = temp + "An error occurred: " + e.Message;   // How to promote this back to client?
                //System.IO.File.WriteAllText("c:\\temp\\myfile.txt", temp);
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
        [HttpPost]
        public JsonResult DeleteConfirmed(string json)
        {
            Item item = Newtonsoft.Json.JsonConvert.DeserializeObject<Item>(json);
       
            if (item.IsRecipe)
            {
                Recipe recipe = _context.Recipe
                        .Where(m => m.RecipeID == item.RecipeID)
                        .Single();
                _context.Recipe.Remove(recipe);
            }
            else
            {
                var list = _context.Recipe.Where(r => r.Ingredients.Any(i => i.item == item))
                    .ToList();
                if(list.Count > 0)
                {
                    return Json("Error: Part of a Recipe");
                }
            }

            _context.Item.Remove(item);
            _context.SaveChanges();
            return Json("Delete confirmed");
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.ItemID == id);
        }
    }
}
