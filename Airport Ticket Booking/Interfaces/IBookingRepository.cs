using Airport_Ticket_Booking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.Interfaces
{
    public interface IBookingRepository
    {
        public void SaveBookings(List<Booking> bookings);
        public List<Booking> GetAllBookings();
    }
}
