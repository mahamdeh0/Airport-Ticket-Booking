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

        public List<Flight> FilterBookings(
            decimal? price = null,
            string departureCountry = null,
            string destinationCountry = null,
            DateTime? departureDate = null,
            string departureAirport = null,
            string arrivalAirport = null
        )
        {
            var flights = _flightRepository.GetAllFlights();

            if (price.HasValue)
                flights = flights.Where(f => f.BasePrice == price.Value).ToList();
            if (!string.IsNullOrEmpty(departureCountry))
                flights = flights.Where(f => f.DepartureCountry.Equals(departureCountry, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!string.IsNullOrEmpty(destinationCountry))
                flights = flights.Where(f => f.DestinationCountry.Equals(destinationCountry, StringComparison.OrdinalIgnoreCase)).ToList();
            if (departureDate.HasValue)
                flights = flights.Where(f => f.DepartureDate.Date == departureDate.Value.Date).ToList();
            if (!string.IsNullOrEmpty(departureAirport))
                flights = flights.Where(f => f.DepartureAirport.Equals(departureAirport, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!string.IsNullOrEmpty(arrivalAirport))
                flights = flights.Where(f => f.ArrivalAirport.Equals(arrivalAirport, StringComparison.OrdinalIgnoreCase)).ToList();

            return flights;
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
