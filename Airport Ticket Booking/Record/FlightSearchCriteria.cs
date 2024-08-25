using Airport_Ticket_Booking.Models.Enums;

namespace Airport_Ticket_Booking.Models
{
    public record FlightSearchCriteria(
        string DepartureCountry,
        string DestinationCountry,
        DateTime DepartureDate,
        string DepartureAirport,
        string ArrivalAirport,
        FlightClass FlightClass
    );
}
