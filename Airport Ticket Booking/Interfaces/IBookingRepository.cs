using Airport_Ticket_Booking.Models;

namespace Airport_Ticket_Booking.Interfaces
{
    public interface IBookingRepository
    {
        public void SaveBookings(List<Booking> bookings);
        public List<Booking> GetAllBookings();
    }
}
