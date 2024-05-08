using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationSystem.Models
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DailySpecial? DailySpecial { get; set; }

        public MenuItem()
        {
            
        }


    }
}
