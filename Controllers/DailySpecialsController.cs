using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantReservationSystem.Data;
using RestaurantReservationSystem.Models;

namespace RestaurantReservationSystem.Controllers
{
    public class DailySpecialsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DailySpecialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DailySpecials
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DailySpecials.Include(d => d.MenuItem);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DailySpecials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailySpecial = await _context.DailySpecials
                .Include(d => d.MenuItem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailySpecial == null)
            {
                return NotFound();
            }

            return View(dailySpecial);
        }

        // GET: DailySpecials/Create
        public IActionResult Create()
        {
            ViewData["MenuId"] = new SelectList(_context.MenuItems, "Id", "Id");
            return View();
        }

        // POST: DailySpecials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Day,MenuId")] DailySpecial dailySpecial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailySpecial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuId"] = new SelectList(_context.MenuItems, "Id", "Id", dailySpecial.MenuId);
            return View(dailySpecial);
        }

        // GET: DailySpecials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailySpecial = await _context.DailySpecials.FindAsync(id);
            if (dailySpecial == null)
            {
                return NotFound();
            }
            ViewData["MenuId"] = new SelectList(_context.MenuItems, "Id", "Id", dailySpecial.MenuId);
            return View(dailySpecial);
        }

        // POST: DailySpecials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Day,MenuId")] DailySpecial dailySpecial)
        {
            if (id != dailySpecial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailySpecial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailySpecialExists(dailySpecial.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuId"] = new SelectList(_context.MenuItems, "Id", "Id", dailySpecial.MenuId);
            return View(dailySpecial);
        }

        // GET: DailySpecials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailySpecial = await _context.DailySpecials
                .Include(d => d.MenuItem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailySpecial == null)
            {
                return NotFound();
            }

            return View(dailySpecial);
        }

        // POST: DailySpecials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dailySpecial = await _context.DailySpecials.FindAsync(id);
            if (dailySpecial != null)
            {
                _context.DailySpecials.Remove(dailySpecial);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailySpecialExists(int id)
        {
            return _context.DailySpecials.Any(e => e.Id == id);
        }
    }
}
