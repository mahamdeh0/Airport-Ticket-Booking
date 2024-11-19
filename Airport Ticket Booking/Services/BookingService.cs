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
        try
        {
            var bookings = _bookingRepository.GetAllBookings();
            bookings.Add(booking);
            _bookingRepository.SaveBookings(bookings);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error booking flight: {ex.Message}");
        }
    }

    public void CancelBooking(int bookingId)
    {
        try
        {
            var bookings = _bookingRepository.GetAllBookings();
            var booking = bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking != null)
            {
                bookings.Remove(booking);
                _bookingRepository.SaveBookings(bookings);
            }
            else
            {
                Console.WriteLine("Booking not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error canceling booking: {ex.Message}");
        }
    }

    public void ModifyBooking(Booking modifiedBooking)
    {
        try
        {
            var bookings = _bookingRepository.GetAllBookings();
            var booking = bookings.FirstOrDefault(b => b.BookingId == modifiedBooking.BookingId);
            if (booking != null)
            {
                bookings.Remove(booking);
                bookings.Add(modifiedBooking);
                _bookingRepository.SaveBookings(bookings);
            }
            else
            {
                Console.WriteLine("Booking not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error modifying booking: {ex.Message}");
        }
    }

    public List<Booking> GetBookingsForPassenger(int passengerId)
    {
        try
        {
            var bookings = _bookingRepository.GetAllBookings();
            return bookings.Where(b => b.PassengerId == passengerId).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving bookings for passenger: {ex.Message}");
            return new List<Booking>();
        }
    }
}
