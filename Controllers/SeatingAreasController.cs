using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantReservationSystem.Data;
using RestaurantReservationSystem.Models;

namespace RestaurantReservationSystem.Controllers
{
    [Authorize(Roles = "Staff")]
    public class SeatingAreasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SeatingAreasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SeatingAreas
        public async Task<IActionResult> Index()
        {
            return View(await _context.SeatingAreas.ToListAsync());
        }

        // GET: SeatingAreas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seatingArea = await _context.SeatingAreas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seatingArea == null)
            {
                return NotFound();
            }

            return View(seatingArea);
        }

        // GET: SeatingAreas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SeatingAreas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] SeatingArea seatingArea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seatingArea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(seatingArea);
        }

        // GET: SeatingAreas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seatingArea = await _context.SeatingAreas.FindAsync(id);
            if (seatingArea == null)
            {
                return NotFound();
            }
            return View(seatingArea);
        }

        // POST: SeatingAreas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] SeatingArea seatingArea)
        {
            if (id != seatingArea.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seatingArea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeatingAreaExists(seatingArea.Id))
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
            return View(seatingArea);
        }

        // GET: SeatingAreas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seatingArea = await _context.SeatingAreas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seatingArea == null)
            {
                return NotFound();
            }

            return View(seatingArea);
        }

        // POST: SeatingAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seatingArea = await _context.SeatingAreas.FindAsync(id);
            if (seatingArea != null)
            {
                _context.SeatingAreas.Remove(seatingArea);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeatingAreaExists(int id)
        {
            return _context.SeatingAreas.Any(e => e.Id == id);
        }
    }
}
