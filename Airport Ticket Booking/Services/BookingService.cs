using Airport_Ticket_Booking.Interfaces;
using Airport_Ticket_Booking.Models;
using Airport_Ticket_Booking.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.Services
{
    public class BookingService : IBookingService
    {
        public void BookFlight(Passenger passenger, Flight flight, FlightClass flightClass)
        {
            throw new NotImplementedException();
        }

        public void CancelBooking(int bookingId)
        {
            throw new NotImplementedException();
        }

        public List<Booking> GetBookingsForPassenger(int passengerId)
        {
            throw new NotImplementedException();
        }

        public void ModifyBooking(Booking modifiedBooking)
        {
            throw new NotImplementedException();
        }
    }
}
