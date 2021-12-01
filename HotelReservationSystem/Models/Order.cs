using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HotelReservationSystem.Models
{
    [Table("Reservation")]
    public class Order
    {
        public int Id { get; set; }

        [Display(Name ="Customer")]
        public int HotelCustomerId { get; set; }

        [Required]
        [Display(Name = "Order Date")]        
        public DateTime DateOrdered { get; set; }

        [Required]
        [Display(Name = "Start Date")]        
        public DateTime StartDate { get; set; }

        
        [Required, Display(Name = "End Date")]        
        public DateTime EndDate { get; set; }

        [Display(Name ="Number of Days")]
        public int NumberOfDays { get; set; }

        [Display(Name = "Cancel Date")]        
        public DateTime? CancelDate { get; set; }

        [Display(Name ="Full Price")]
        [DisplayFormat(DataFormatString = "{0} EGP", ApplyFormatInEditMode = false)]
        public decimal FullPrice { get; set; }

        [Display(Name = "Cancel Price")]
        [DisplayFormat(DataFormatString = "{0} EGP", ApplyFormatInEditMode = false)]
        public decimal? CancelPrice { get; set; }

        [Required, Display(Name = "Hotel")]
        public int HotelId { get; set; }

        [Required, Display(Name = "Room")]
        public int RoomId { get; set; }

        [Display(Name = "Ordered By")]
        public string RequesterUserId { get; set; }

        [Display(Name = "Canceled By")]
        public string CancelUserId { get; set; }
        public string UpdatedByHotelAdminUserId { get; set; }

        public Hotel Hotel { get; set; }
        public Room Room { get; set; }

        public Customer HotelCustomer { get; set; }

        public ApplicationUser RequesterUser { get; set; }
        public ApplicationUser CancelUser { get; set; }
        public ApplicationUser UpdatedByHotelAdminUser { get; set; }
    }

    
}