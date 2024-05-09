using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantReservationSystem.Data;
using RestaurantReservationSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


namespace RestaurantReservationSystem.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        public ReservationsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }


            if (!User.IsInRole("Staff"))
            {
                var applicationDbContext = _context.Reservations
                .Where(u => u.IdentityUserId == currentUser.Id)
                .Include(r => r.User).Include(t => t.RestaurantTable)
                .Include(s => s.RestaurantTable.SeatingArea);
                return View(await applicationDbContext.ToListAsync());

            }

            var applicationDbContextStaff = _context.Reservations
            .Include(r => r.User).Include(t => t.RestaurantTable)
            .Include(s => s.RestaurantTable.SeatingArea);

            return View(await applicationDbContextStaff.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        //public IActionResult Create()
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return NotFound();
            }
            ViewData["IdentityUserId"] = currentUser.Id;

            var availableTables = _context.RestaurantTables.Where(t => t.isAvailable == true).Include(s => s.SeatingArea).ToList();

            List<SelectListItem> availableTablesList = availableTables.Select(table => new SelectListItem
            {
                Text =  table.SeatingArea.Name + " - " + table.Capacity.ToString(),
                Value = table.Id.ToString(),
            }).ToList();

            ViewData["RestaurantTableId"] = availableTablesList;
            //ViewData["RestaurantTableId"] = new SelectList(_context.RestaurantTables, "Id", "Id");


            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CheckIn,CheckOut,isCancelled,IdentityUserId, RestaurantTableId")] Reservation reservation)
        {
            ModelState.Clear();

            reservation.RestaurantTable = await _context.RestaurantTables.FirstOrDefaultAsync(r => r.Id == reservation.RestaurantTableId);
            reservation.RestaurantTable.SeatingArea = await _context.SeatingAreas.FirstOrDefaultAsync(r => r.Id == reservation.RestaurantTable.SeatingAreaId);
            reservation.User = await _userManager.FindByIdAsync(reservation.IdentityUserId);



            if (!TryValidateModel(reservation, nameof(reservation)))
            {

                ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", reservation.IdentityUserId);
                ViewData["RestaurantTableId"] = new SelectList(_context.RestaurantTables, "Id", "Id");
                TempData["status"] = " Reservation Failed";
                return View(reservation);
            }


       
            _context.Add(reservation);
            await _context.SaveChangesAsync();
            TempData["status"] = reservation.RestaurantTable.SeatingArea.Name.ToString() + " - " + reservation.RestaurantTable.Capacity.ToString() + " Reserved Successfully";
            return RedirectToAction(nameof(Index));
            

        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["IdentityUserId"] = reservation.IdentityUserId;
            //ViewData["RestaurantTableId"] = new SelectList(_context.RestaurantTables, "Id", "Id");

            var availableTables = _context.RestaurantTables.Where(t => t.isAvailable == true).Include(s => s.SeatingArea).ToList();

            List<SelectListItem> availableTablesList = availableTables.Select(table => new SelectListItem
            {
                Text = table.SeatingArea.Name + " - " + table.Capacity.ToString(),
                Value = table.Id.ToString(),
            }).ToList();

            ViewData["RestaurantTableId"] = availableTablesList;

            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CheckIn,CheckOut,isCancelled,IdentityUserId, RestaurantTableId")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            ModelState.Clear();

            reservation.RestaurantTable = await _context.RestaurantTables.FirstOrDefaultAsync(r => r.Id == reservation.RestaurantTableId);
            reservation.RestaurantTable.SeatingArea = await _context.SeatingAreas.FirstOrDefaultAsync(r => r.Id == reservation.RestaurantTable.SeatingAreaId);
            reservation.User = await _userManager.FindByIdAsync(reservation.IdentityUserId);

            if (!TryValidateModel(reservation, nameof(reservation)))
            {

                ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", reservation.IdentityUserId);
                ViewData["RestaurantTableId"] = new SelectList(_context.RestaurantTables, "Id", "Id");
                TempData["status"] = " Reservation Edit Failed";
                return View(reservation);
            }

     
            try
            {
                _context.Update(reservation);
                await _context.SaveChangesAsync();
                TempData["status"] = reservation.RestaurantTable.SeatingArea.Name.ToString() + " - " + reservation.RestaurantTable.Capacity.ToString() + "Reservation Edited Successfully";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(reservation.Id))
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


        // GET: Reservations/Report
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Report()
        {
         
            var reservations = await _context.Reservations
                .Include(u => u.User)
                .Include(t => t.RestaurantTable)
                .Include(s => s.RestaurantTable.SeatingArea)
                .ToListAsync();

            // Group reservations by day of the week and count the number of reservations for each day
            var bookingFrequency = reservations
                .GroupBy(r => r.CheckIn.DayOfWeek)
                .Select(g => new
                {
                    DayOfWeek = g.Key,
                    Frequency = g.Count()
                })
                .OrderBy(g => g.DayOfWeek)
                .ToList();

            ViewData["BookingFrequency"] = bookingFrequency;


            // Group reservations by hour of the day and count the number of reservations for each hour
            var bookingFrequencyByHour = reservations
                .GroupBy(r => r.CheckIn.Hour)
                .Select(g => new
                {
                    HourOfDay = g.Key,
                    Frequency = g.Count()
                })
                .OrderBy(g => g.HourOfDay)
                .ToList();

            ViewData["BookingFrequencyByHour"] = bookingFrequencyByHour;

            return View(reservations);
        }


        

        //// GET: Reservations/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var reservation = await _context.Reservations
        //        .Include(r => r.User)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (reservation == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(reservation);
        //}

        //// POST: Reservations/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var reservation = await _context.Reservations.FindAsync(id);
        //    if (reservation != null)
        //    {
        //        _context.Reservations.Remove(reservation);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
