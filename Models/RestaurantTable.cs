using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantReservationSystem.Models
{
    public class RestaurantTable
    {
        [Key]
        public int Id { get; set; }
        public int Capacity { get; set; }
        public bool isAvailable { get; set; }
        public Reservation? Reservation { get; set; }

        [Required]
        public SeatingArea SeatingArea { get; set; }

        public RestaurantTable()
        {
            
        }
    }
}
