using Airport_Ticket_Booking.Models;

namespace Airport_Ticket_Booking.Interfaces
{
    public interface IFlightRepository
    {
        public void SaveFlights(List<Flight> flights);
        public List<Flight> GetAllFlights();
    }
}
