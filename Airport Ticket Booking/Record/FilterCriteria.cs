namespace Airport_Ticket_Booking.Record
{
    public record FilterCriteria(
      decimal? Price = null,
      string DepartureCountry = null,
      string DestinationCountry = null,
      DateTime? DepartureDate = null,
      string DepartureAirport = null,
      string ArrivalAirport = null
  );

}
