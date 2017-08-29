using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VeganPlanner.Models;

namespace VeganPlanner.Controllers
{
    public class StepsController : Controller
    {
        private readonly VeganPlannerContext _context;

        public StepsController(VeganPlannerContext context)
        {
            _context = context;    
        }

        // GET: Steps
        public async Task<IActionResult> Index()
        {
            return View(await _context.Step.ToListAsync());
        }

        // GET: Steps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var step = await _context.Step
                .SingleOrDefaultAsync(m => m.StepID == id);
            if (step == null)
            {
                return NotFound();
            }

            return View(step);
        }

        // GET: Steps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Steps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StepID,RecipeID,Description,Order")] Step step)
        {
            if (ModelState.IsValid)
            {
                _context.Add(step);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(step);
        }

        // GET: Steps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var step = await _context.Step.SingleOrDefaultAsync(m => m.StepID == id);
            if (step == null)
            {
                return NotFound();
            }
            return View(step);
        }

        // POST: Steps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StepID,RecipeID,Description,Order")] Step step)
        {
            if (id != step.StepID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(step);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StepExists(step.StepID))
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
            return View(step);
        }

        // GET: Steps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var step = await _context.Step
                .SingleOrDefaultAsync(m => m.StepID == id);
            if (step == null)
            {
                return NotFound();
            }

            return View(step);
        }

        // POST: Steps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var step = await _context.Step.SingleOrDefaultAsync(m => m.StepID == id);
            _context.Step.Remove(step);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool StepExists(int id)
        {
            return _context.Step.Any(e => e.StepID == id);
        }
    }
}
