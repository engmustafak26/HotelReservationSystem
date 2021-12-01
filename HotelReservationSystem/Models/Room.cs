using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelReservationSystem.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(650)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Range(1, 3)]
        [Display(Name = "Number Of Beds")]
        public int BedsCount { get; set; }

        [Required]
        [Range(1, 10000)]
        [DisplayFormat(DataFormatString = "{0} EGP", ApplyFormatInEditMode = false)]
        [Display(Name = "Price Per Night")]
        public double PricePerNight { get; set; }

        [Required]
        [Display(Name = "Deactivate")]
        public bool IsInactive { get; set; }

        [Required]
        [Display(Name = "Hotel")]
        public int HotelId { get; set; }

        public Hotel Hotel { get; set; }
    }
}