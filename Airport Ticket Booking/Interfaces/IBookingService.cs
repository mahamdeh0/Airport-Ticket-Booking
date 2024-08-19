using Airport_Ticket_Booking.Models.Enums;
using Airport_Ticket_Booking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.Interfaces
{
    public interface IBookingService
    {
        public void BookFlight(Booking booking);
        public void CancelBooking(int bookingId);
        public void ModifyBooking(Booking modifiedBooking);
        public List<Booking> GetBookingsForPassenger(int passengerId);
    }
}
