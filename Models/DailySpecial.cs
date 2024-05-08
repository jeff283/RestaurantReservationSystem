using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantReservationSystem.Models
{
    public class DailySpecial
    {
        [Key]
        public int Id { get; set; }
        public string Day { get; set; }

        //[Required]
        [ForeignKey("MenuItem")]
        public int MenuId { get; set; }
        public MenuItem MenuItem { get; set; }

        public DailySpecial()
        {
            
        }

    }
}
