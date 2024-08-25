using Airport_Ticket_Booking.Models;
using Airport_Ticket_Booking.Models.Enums;

namespace Airport_Ticket_Booking.Interfaces
{
    public interface IPassengerService
    {
        public void BookFlight(Passenger passenger, Flight flight, FlightClass flightClass);
        public List<Flight> SearchAvailableFlights(FlightSearchCriteria criteria);
        public void CancelBooking(int bookingId);
        public void ModifyBooking(Booking newBooking);
        public List<Booking> ViewPersonalBookings(int passengerId);
    }
}
