using Airport_Ticket_Booking.Interfaces;
using Airport_Ticket_Booking.Models;
using Airport_Ticket_Booking.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IBookingService _bookingService;
        private readonly IFlightRepository _flightRepository;

        public PassengerService(IBookingService bookingService , IFlightRepository flightRepository)
        {
            _bookingService = bookingService;
            _flightRepository = flightRepository;
        }

        public void BookFlight(Passenger passenger, Flight flight, FlightClass flightClass)
        {
            var booking = new Booking
            {
                BookingId = GenerateBookingId(),
                FlightId = flight.FlightNumber,
                PassengerId = passenger.Id,
                Class = flightClass,
                Price = CalculatePrice(flight.BasePrice, flightClass)
            };

            _bookingService.BookFlight(booking);
        }

        public void CancelBooking(int bookingId)
        {
            _bookingService.CancelBooking(bookingId);
        }

        public void ModifyBooking(Booking newBooking)
        {
            _bookingService.ModifyBooking(newBooking);
        }

        public List<Flight> SearchAvailableFlights(string departureCountry, string destinationCountry, DateTime departureDate, string departureAirport, string arrivalAirport, FlightClass flightClass)
        {
            var flights = _flightRepository.GetAllFlights();
            return flights
                .Where(f => f.DepartureCountry == departureCountry && f.DestinationCountry == destinationCountry &&
                            f.DepartureDate.Date == departureDate.Date && f.DepartureAirport == departureAirport &&
                            f.ArrivalAirport == arrivalAirport)
                .ToList();
        }

        public List<Booking> ViewPersonalBookings(int passengerId)
        {
            return _bookingService.GetBookingsForPassenger(passengerId);
        }

        private decimal CalculatePrice(decimal basePrice, FlightClass flightClass)
        {
            switch (flightClass)
            {
                case FlightClass.Economy:
                    return basePrice;
                case FlightClass.Business:
                    return basePrice * 1.5m;
                case FlightClass.FirstClass:
                    return basePrice * 2.0m;
                default:
                    Console.WriteLine("Invalid flight class.");
                    return -1;
            }
        }

        private int GenerateBookingId()
        {
            return new Random().Next(1, 99999);
        }
    }
}
