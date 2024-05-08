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
    public class RestaurantTablesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantTablesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RestaurantTables
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RestaurantTables.Include(s => s.SeatingArea);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RestaurantTables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantTable = await _context.RestaurantTables
                .Include(d => d.SeatingArea)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurantTable == null)
            {
                return NotFound();
            }

            return View(restaurantTable);
        }

        // GET: RestaurantTables/Create
        //public IActionResult Create()
        //{
        //    ViewData["SeatingAreaId"] = new SelectList(_context.SeatingAreas, "Id", "Id");
        //    return View();
        //}
        public IActionResult Create()
        {
            // Retrieve the seating areas from the database
            var seatingAreas = _context.SeatingAreas.ToList();

            // Create a list of SelectListItem objects with desired label and value pairs
            List<SelectListItem> seatingAreaList = seatingAreas.Select(area => new SelectListItem
            {
                Text = area.Name,  // Assuming AreaName is the property you want as the label
                Value = area.Id.ToString()  // Assuming Id is the property you want as the value
            }).ToList();

            // Set ViewBag.SeatingAreaId to the list of SelectListItem objects
            ViewBag.SeatingAreaId = seatingAreaList;

            return View();


        }

        // POST: RestaurantTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Capacity,isAvailable, SeatingAreaId")] RestaurantTable restaurantTable)
        {
            ModelState.Clear();


            restaurantTable.SeatingArea = _context.SeatingAreas.FirstOrDefault(m => m.Id == restaurantTable.SeatingAreaId);

            if (!TryValidateModel(restaurantTable, nameof(restaurantTable)))
            {
                // Retrieve the seating areas from the database
                var seatingAreas = _context.SeatingAreas.ToList();

                // Create a list of SelectListItem objects with desired label and value pairs
                List<SelectListItem> seatingAreaList = seatingAreas.Select(area => new SelectListItem
                {
                    Text = area.Name,  // Assuming AreaName is the property you want as the label
                    Value = area.Id.ToString()  // Assuming Id is the property you want as the value
                }).ToList();

                // Set ViewBag.SeatingAreaId to the list of SelectListItem objects
                ViewBag.SeatingAreaId = seatingAreaList;
                return View(restaurantTable);
            }

            _context.Add(restaurantTable);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: RestaurantTables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantTable = await _context.RestaurantTables.FindAsync(id);
            if (restaurantTable == null)
            {
                return NotFound();
            }

            var seatingAreas = _context.SeatingAreas.ToList();

            // Create a list of SelectListItem objects with desired label and value pairs
            List<SelectListItem> seatingAreaList = seatingAreas.Select(area => new SelectListItem
            {
                Text = area.Name,  // Assuming AreaName is the property you want as the label
                Value = area.Id.ToString()  // Assuming Id is the property you want as the value
            }).ToList();

            // Set ViewBag.SeatingAreaId to the list of SelectListItem objects
            ViewBag.SeatingAreaId = seatingAreaList;
            return View(restaurantTable);

        }

        // POST: RestaurantTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Capacity,isAvailable, SeatingAreaId")] RestaurantTable restaurantTable)
        {
            if (id != restaurantTable.Id)
            {
                return NotFound();
            }


            ModelState.Clear();
            restaurantTable.SeatingArea = _context.SeatingAreas.FirstOrDefault(m => m.Id == restaurantTable.SeatingAreaId);

            if (!TryValidateModel(restaurantTable, nameof(restaurantTable)))
            {
                // Retrieve the seating areas from the database
                var seatingAreas = _context.SeatingAreas.ToList();

                // Create a list of SelectListItem objects with desired label and value pairs
                List<SelectListItem> seatingAreaList = seatingAreas.Select(area => new SelectListItem
                {
                    Text = area.Name,  // Assuming AreaName is the property you want as the label
                    Value = area.Id.ToString()  // Assuming Id is the property you want as the value
                }).ToList();

                // Set ViewBag.SeatingAreaId to the list of SelectListItem objects
                ViewBag.SeatingAreaId = seatingAreaList;
                return View(restaurantTable);
            }

            try
            {
                _context.Update(restaurantTable);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantTableExists(restaurantTable.Id))
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

        // GET: RestaurantTables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantTable = await _context.RestaurantTables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurantTable == null)
            {
                return NotFound();
            }

            return View(restaurantTable);
        }

        // POST: RestaurantTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurantTable = await _context.RestaurantTables.FindAsync(id);
            if (restaurantTable != null)
            {
                _context.RestaurantTables.Remove(restaurantTable);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantTableExists(int id)
        {
            return _context.RestaurantTables.Any(e => e.Id == id);
        }
    }
}
