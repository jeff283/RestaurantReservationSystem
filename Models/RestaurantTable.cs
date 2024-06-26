﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantReservationSystem.Models
{
    public class RestaurantTable
    {
        [Key]
        public int Id { get; set; }
        public int Capacity { get; set; }
        public bool isAvailable { get; set; } = true;


        public ICollection<Reservation>? Reservation { get; set; }


        [Required]
        public int SeatingAreaId { get; set; }

        [ForeignKey("SeatingAreaId")]
        public SeatingArea SeatingArea { get; set; }

        public RestaurantTable()
        {
            
        }
    }
}
