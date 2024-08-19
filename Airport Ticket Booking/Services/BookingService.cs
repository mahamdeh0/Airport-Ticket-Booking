using Airport_Ticket_Booking.Interfaces;
using Airport_Ticket_Booking.Models;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public void BookFlight(Booking booking)
    {
        var bookings = _bookingRepository.GetAllBookings();
        bookings.Add(booking);
        _bookingRepository.SaveBookings(bookings);
    }

    public void CancelBooking(int bookingId)
    {
        var bookings = _bookingRepository.GetAllBookings();
        var booking = bookings.FirstOrDefault(b => b.BookingId == bookingId);
        if (booking != null)
        {
            bookings.Remove(booking);
            _bookingRepository.SaveBookings(bookings);
        }
    }
    public void ModifyBooking(Booking modifiedBooking)
    {
        var bookings = _bookingRepository.GetAllBookings();
        var booking = bookings.FirstOrDefault(b => b.BookingId == modifiedBooking.BookingId);
        if (booking != null)
        {
            bookings.Remove(booking);
            bookings.Add(modifiedBooking);
            _bookingRepository.SaveBookings(bookings);
        }
    }

    public List<Booking> GetBookingsForPassenger(int passengerId)
    {
        var bookings = _bookingRepository.GetAllBookings();
        return bookings.Where(b => b.PassengerId == passengerId).ToList();
    }


}
