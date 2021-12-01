using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelReservationSystem.Models
{
    public class JqueryDatatableParam
    {
        public string Search { get; set; }
        public string Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
 
    }
}