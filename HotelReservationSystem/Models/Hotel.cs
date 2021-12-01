using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HotelReservationSystem.Models
{
    public class Hotel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public Country Country { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [Range(1,5)]
        public int Stars { get; set; }

        [Display(Name = "Deactivate")]
        public bool IsInactive { get; set; }

        [Required]
        [Display(Name = "Free Cancelation - Days Before Reservation Date")]
        [Range(0,30)]
        public int FreeCancelationDaysBeforeReservationDate { get; set; }

        [Required]
        [Display(Name = "Deduction Percentage For Reservation Cancelation")]
        [DisplayFormat(DataFormatString = "{0} %", ApplyFormatInEditMode = false)]
        [Range(0, 100)]
        public decimal DeductionPercentageForReservationCancelation { get; set; }

        [Required]
        [Display(Name = "Check-in Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public TimeSpan CheckinTime { get; set; } = new TimeSpan(10, 0, 0);

        [Required]        
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
        
    }
}