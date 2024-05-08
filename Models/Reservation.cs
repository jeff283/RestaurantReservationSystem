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

        [Required]
        public int RestaurantTableId { get; set; }
        [ForeignKey("RestaurantTableId")]
        public RestaurantTable RestaurantTable { get; set;}

        [Required]
        public string IdentityUserId { get; set; }
        [ForeignKey("IdentityUserId")]
        public IdentityUser User { get; set; }

    }
}
