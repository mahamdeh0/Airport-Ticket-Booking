using Airport_Ticket_Booking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.Interfaces
{
    public interface IBookingManager
    {
        public void ImportFlightsFromCsv(string filePath);

        List<Flight> FilterBookings(
           decimal? price = null,
           string departureCountry = null,
           string destinationCountry = null,
           DateTime? departureDate = null,
           string departureAirport = null,
           string arrivalAirport = null
       );

    }
}
