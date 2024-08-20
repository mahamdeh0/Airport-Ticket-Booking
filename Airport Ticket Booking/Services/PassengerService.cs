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

        public PassengerService(IBookingService bookingService, IFlightRepository flightRepository)
        {
            _bookingService = bookingService;
            _flightRepository = flightRepository;
        }

        public void BookFlight(Passenger passenger, Flight flight, FlightClass flightClass)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error booking flight: {ex.Message}");
            }
        }

        public void CancelBooking(int bookingId)
        {
            try
            {
                _bookingService.CancelBooking(bookingId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error canceling booking: {ex.Message}");
            }
        }

        public void ModifyBooking(Booking newBooking)
        {
            try
            {
                _bookingService.ModifyBooking(newBooking);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error modifying booking: {ex.Message}");
            }
        }

        public List<Flight> SearchAvailableFlights(string departureCountry, string destinationCountry, DateTime departureDate, string departureAirport, string arrivalAirport, FlightClass flightClass)
        {
            try
            {
                var flights = _flightRepository.GetAllFlights();
                return flights
                    .Where(f => f.DepartureCountry == departureCountry && f.DestinationCountry == destinationCountry &&
                                f.DepartureDate.Date == departureDate.Date && f.DepartureAirport == departureAirport &&
                                f.ArrivalAirport == arrivalAirport)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching for flights: {ex.Message}");
                return new List<Flight>();
            }
        }

        public List<Booking> ViewPersonalBookings(int passengerId)
        {
            try
            {
                return _bookingService.GetBookingsForPassenger(passengerId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error viewing personal bookings: {ex.Message}");
                return new List<Booking>();
            }
        }

        public decimal CalculatePrice(decimal basePrice, FlightClass flightClass)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating price: {ex.Message}");
                return -1;
            }
        }

        private int GenerateBookingId()
        {
            try
            {
                return new Random().Next(1, 99999);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating booking ID: {ex.Message}");
                return -1;
            }
        }
    }
}
