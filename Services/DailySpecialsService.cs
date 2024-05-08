using Microsoft.EntityFrameworkCore;
using RestaurantReservationSystem.Data;
using RestaurantReservationSystem.Interfaces;
using RestaurantReservationSystem.Models;

namespace RestaurantReservationSystem.Services
{
    public class DailySpecialsService: IDailySpecialsService
    {
        private readonly ApplicationDbContext _context;

        public DailySpecialsService(ApplicationDbContext context)
        {
            _context = context;
        }


        public IQueryable<DailySpecial> GetAll()
        {
            var applicationDbContext = _context.DailySpecials.Include(d => d.MenuItem);
            return applicationDbContext;
        }

        public async Task Add(DailySpecial dailySpecial)
        {
            _context.DailySpecials.Add(dailySpecial);
            await _context.SaveChangesAsync();
        }
    }
}
