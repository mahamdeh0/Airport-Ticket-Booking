using Airport_Ticket_Booking.Models;
using Airport_Ticket_Booking.Record;

namespace Airport_Ticket_Booking.Interfaces
{
    public interface IBookingManager
    {
        public void ImportFlightsFromCsv(string filePath);
        List<Flight> FilterBookings(FilterCriteria criteria);
    
    }
}
