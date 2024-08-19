using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.Models
{
    public class Passenger
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();

    }
}
