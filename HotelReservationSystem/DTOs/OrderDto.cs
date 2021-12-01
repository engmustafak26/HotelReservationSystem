using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelReservationSystem.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Customer")]
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Hotel")]
        public int HotelId { get; set; }

        [Required]
        [Display(Name = "Order Date")]
        public DateTime DateOrdered { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required,Display(Name ="End Date")]
        public DateTime EndDate { get; set; }        

        [Required,Display(Name ="Room")]
        public int RoomId { get; set; }
    }
}