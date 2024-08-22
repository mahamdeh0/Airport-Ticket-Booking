using Airport_Ticket_Booking.Models;

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
