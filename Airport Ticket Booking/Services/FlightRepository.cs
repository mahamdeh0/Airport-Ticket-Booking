using Airport_Ticket_Booking.Interfaces;
using Airport_Ticket_Booking.Models;

namespace Airport_Ticket_Booking.Services
{
    public class FlightRepository : IFlightRepository
    {
        private readonly IFileStorage _fileStorage;
        private readonly string _filePath = @"C:\Users\maham\Desktop\Airport Ticket Booking\Airport Ticket Booking\flights.csv";

        public FlightRepository(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }

        public List<Flight> GetAllFlights()
        {
            try
            {
                return _fileStorage.ReadFromFile<Flight>(_filePath);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while reading the flights file: {ex.Message}");
                return new List<Flight>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred while retrieving flights: {ex.Message}");
                return new List<Flight>();
            }
        }

        public void SaveFlights(List<Flight> flights)
        {
            try
            {
                _fileStorage.WriteToFile(flights, _filePath);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while writing to the flights file: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred while saving flights: {ex.Message}");
            }
        }
    }
}
