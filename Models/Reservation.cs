using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantReservationSystem.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public bool isCancelled { get; set; } = false;

        public ICollection<RestaurantTable> RestaurantTables { get; set;}

        [Required]
        public string IdentityUserId { get; set; }
        [ForeignKey("IdentityUserId")]
        public IdentityUser User { get; set; }

    }
}
