using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationSystem.Models
{
    public class SeatingArea
    {
        [Key]
        public int Id { get; set; }
        public string Name{ get; set; }

        public ICollection<RestaurantTable>? RestaurantTables { get; set; }
        public SeatingArea()
        {
            
        }
    }
}
