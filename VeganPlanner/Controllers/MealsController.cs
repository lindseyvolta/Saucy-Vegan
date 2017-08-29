using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VeganPlanner.Models;
using Microsoft.AspNetCore.Identity;


namespace VeganPlanner.Controllers
{
    public class MealsController : Controller
    {
        private readonly VeganPlannerContext _context;
        private UserManager<ApplicationUser> _userManager;

        public MealsController(VeganPlannerContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // Requires using Microsoft.AspNetCore.Mvc.Rendering;
        public ActionResult Index()
        {
            return View("Index");
        }

        public async Task<IActionResult> GetMeals(string searchString)
        {
            var user = await _userManager.GetUserAsync(User);
            var username = user.UserName;

            var meals = from m in _context.Meal
                       .Include(c => c.MealComponents)
                         .AsNoTracking()
                        where m.UserID == username || m.UserID == "lvolta@umich.edu" || m.UserID == "jvolta@vtechnologies.com"
                        select m;

            if (!String.IsNullOrEmpty(searchString))
                meals = meals.Where(s => s.NickName.Contains(searchString));

            return Json(new { meals = await meals.OrderBy(x => x.NickName).ToListAsync() });
        }

        // GET: Meals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meal
                .SingleOrDefaultAsync(m => m.MealID == id);
            if (meal == null)
            {
                return NotFound();
            }

            return View(meal);
        }

        // GET: Meals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Meals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MealID,NickName,UserID,CaloriesPerServing,ProteinPerServing,Notes")] Meal meal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(meal);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(meal);
        }

        // GET: Meals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meal.SingleOrDefaultAsync(m => m.MealID == id);
            if (meal == null)
            {
                return NotFound();
            }
            return View(meal);
        }

        // POST: Meals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MealID,NickName,UserID,CaloriesPerServing,ProteinPerServing,Notes")] Meal meal)
        {
            if (id != meal.MealID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MealExists(meal.MealID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(meal);
        }

        // GET: Meals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meal
                .SingleOrDefaultAsync(m => m.MealID == id);
            if (meal == null)
            {
                return NotFound();
            }

            return View(meal);
        }

        // POST: Meals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meal = await _context.Meal.SingleOrDefaultAsync(m => m.MealID == id);
            _context.Meal.Remove(meal);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MealExists(int id)
        {
            return _context.Meal.Any(e => e.MealID == id);
        }
    }
}
