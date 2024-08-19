using Airport_Ticket_Booking.Interfaces;
using Airport_Ticket_Booking.Models;

namespace Airport_Ticket_Booking.Services
{
    public class BookingManager : IBookingManager
    {
        private readonly IFlightRepository _flightRepository;

        public BookingManager(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public void ImportFlightsFromCsv(string filePath)
        {
            var flights = new List<Flight>();


            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines.Skip(1)) 
            {
                var values = line.Split(',');

                var flight = new Flight
                {
                    FlightNumber = int.Parse(values[0]),
                    DepartureCountry = values[1],
                    DestinationCountry = values[2],
                    DepartureDate = DateTime.Parse(values[3]),
                    DepartureAirport = values[4],
                    ArrivalAirport = values[5],
                    BasePrice = decimal.Parse(values[6])
                };

                flights.Add(flight);
            }

            _flightRepository.SaveFlights(flights);
        }
    }

}
