using Airport_Ticket_Booking.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int FlightId { get; set; }
        public int PassengerId { get; set; }
        public FlightClass Class { get; set; }
        public decimal Price { get; set; }
    }
}

