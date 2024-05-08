using RestaurantReservationSystem.Models;

namespace RestaurantReservationSystem.Interfaces
{
    public interface IDailySpecialsService
    {
        IQueryable<DailySpecial> GetAll();
        Task Add(DailySpecial dailySpecial);
    }
}
