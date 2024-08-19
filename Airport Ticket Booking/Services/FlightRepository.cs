using Airport_Ticket_Booking.Interfaces;
using Airport_Ticket_Booking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.Services
{
    internal class FlightRepository : IFlightRepository
    {
        private readonly IFileStorage _fileStorage;
        private readonly string _filePath = @"C:\Users\maham\Desktop\Airport Ticket Booking\Airport Ticket Booking\flights.csv";

        public FlightRepository(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }

        public List<Flight> GetAllFlights()
        {
            return _fileStorage.ReadFromFile<Flight>(_filePath);
        }

        public void SaveFlights(List<Flight> flights)
        {
            _fileStorage.WriteToFile(flights, _filePath);

        }
    }
}
