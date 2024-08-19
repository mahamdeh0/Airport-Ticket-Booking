using System;
using System.Collections.Generic;
using Airport_Ticket_Booking.Models;
using Airport_Ticket_Booking.Models.Enums;

namespace Airport_Ticket_Booking.Interfaces
{
    public interface IPassengerService
    {
        public void BookFlight(Passenger passenger, Flight flight, FlightClass flightClass);
        public List<Flight> SearchAvailableFlights(string departureCountry, string destinationCountry, DateTime departureDate, string departureAirport, string arrivalAirport, FlightClass flightClass); // Search for available flights
        public void CancelBooking(int bookingId);
        public void ModifyBooking(Booking newBooking);
        public List<Booking> ViewPersonalBookings(int passengerId); 
    }
}