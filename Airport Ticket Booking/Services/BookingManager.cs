using Airport_Ticket_Booking.Interfaces;
using Airport_Ticket_Booking.Models;
using Airport_Ticket_Booking.Record;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Airport_Ticket_Booking.Services
{
    public class BookingManager : IBookingManager
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IValidationService _validationService;

        public BookingManager(IFlightRepository flightRepository, IValidationService validationService)
        {
            _flightRepository = flightRepository;
            _validationService = validationService;
        }

        public List<Flight> FilterBookings(FilterCriteria criteria)
        {
            try
            {
                var flights = _flightRepository.GetAllFlights();

                if (criteria.Price.HasValue)
                    flights = flights.Where(f => f.BasePrice == criteria.Price.Value).ToList();
                if (!string.IsNullOrEmpty(criteria.DepartureCountry))
                    flights = flights.Where(f => f.DepartureCountry.Equals(criteria.DepartureCountry, StringComparison.OrdinalIgnoreCase)).ToList();
                if (!string.IsNullOrEmpty(criteria.DestinationCountry))
                    flights = flights.Where(f => f.DestinationCountry.Equals(criteria.DestinationCountry, StringComparison.OrdinalIgnoreCase)).ToList();
                if (criteria.DepartureDate.HasValue)
                    flights = flights.Where(f => f.DepartureDate.Date == criteria.DepartureDate.Value.Date).ToList();
                if (!string.IsNullOrEmpty(criteria.DepartureAirport))
                    flights = flights.Where(f => f.DepartureAirport.Equals(criteria.DepartureAirport, StringComparison.OrdinalIgnoreCase)).ToList();
                if (!string.IsNullOrEmpty(criteria.ArrivalAirport))
                    flights = flights.Where(f => f.ArrivalAirport.Equals(criteria.ArrivalAirport, StringComparison.OrdinalIgnoreCase)).ToList();

                return flights;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while filtering bookings: {ex.Message}");
                return new List<Flight>();
            }
        }

        public void ImportFlightsFromCsv(string filePath)
        {
            try
            {
                var flights = new List<Flight>();
                var errors = new List<string>();

                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines.Skip(1))
                {
                    var values = line.Split(',');

                    if (values.Length != 7)
                    {
                        errors.Add($"Invalid data format: {line}");
                        continue;
                    }

                    var flight = new Flight();
                    try
                    {
                        flight.FlightNumber = int.Parse(values[0]);
                        flight.DepartureCountry = values[1];
                        flight.DestinationCountry = values[2];
                        flight.DepartureDate = DateTime.Parse(values[3]);
                        flight.DepartureAirport = values[4];
                        flight.ArrivalAirport = values[5];
                        flight.BasePrice = decimal.Parse(values[6]);

                        var validationResults = _validationService.ValidateFlight(flight);
                        if (validationResults.Any())
                        {
                            foreach (var result in validationResults)
                            {
                                errors.Add($"{line}: {result.ErrorMessage}");
                            }
                        }
                        else
                        {
                            flights.Add(flight);
                        }
                    }
                    catch (FormatException ex)
                    {
                        errors.Add($"Error parsing line '{line}': {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"Unexpected error parsing line '{line}': {ex.Message}");
                    }
                }

                if (errors.Any())
                {
                    Console.WriteLine("Errors during import:");
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error);
                    }
                }
                else
                {
                    _flightRepository.SaveFlights(flights);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred during import: {ex.Message}");
            }
        }
    }
}
