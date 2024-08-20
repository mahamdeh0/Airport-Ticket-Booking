using Airport_Ticket_Booking.Interfaces;
using Airport_Ticket_Booking.Models.Enums;
using Airport_Ticket_Booking.Models;
using Airport_Ticket_Booking.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.Utilities
{
    public class Menu
    {
        private readonly IBookingService _bookingService;
        private readonly IFlightRepository _flightRepository;
        private readonly IPassengerService _passengerService;
        private readonly IValidationService _validationService;

        public Menu(IBookingService bookingService, IFlightRepository flightRepository, IPassengerService passengerService, IValidationService validationService)
        {
            _bookingService = bookingService;
            _flightRepository = flightRepository;
            _passengerService = passengerService;
            _validationService = validationService;
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Airport Ticket Booking System");
                Console.WriteLine("1. Book a Flight");
                Console.WriteLine("2. View Personal Bookings");
                Console.WriteLine("3. Modify a Booking");
                Console.WriteLine("4. Cancel a Booking");
                Console.WriteLine("5. Search Available Flights");
                Console.WriteLine("6. View All Available Flights");
                Console.WriteLine("7. Import Flights from CSV");
                Console.WriteLine("8. Exit");
                Console.Write("Select an option: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        BookFlight();
                        break;
                    case "2":
                        ViewPersonalBookings();
                        break;
                    case "3":
                        ModifyBooking();
                        break;
                    case "4":
                        CancelBooking();
                        break;
                    case "5":
                        SearchAvailableFlights();
                        break;
                    case "6":
                        ViewAllAvailableFlights();
                        break;
                    case "7":
                        ImportFlightsFromCsv();
                        break;
                    case "8":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void BookFlight()
        {
            try
            {
                var flightRepository = _flightRepository.GetAllFlights();

                if (flightRepository.Count > 0)
                {
                    Console.WriteLine("All available flights:");
                    foreach (var flying in flightRepository)
                    {
                        Console.WriteLine($"Flight Number: {flying.FlightNumber}, {flying.DepartureCountry}->{flying.DestinationCountry} , Departure: {flying.DepartureAirport} -> Arrival: {flying.ArrivalAirport}, Date: {flying.DepartureDate.ToShortDateString()}, Price: {flying.BasePrice}");
                    }
                }
                else
                {
                    Console.WriteLine("No flights available");
                    return;
                }

                int passengerId;
                while (true)
                {
                    Console.Write("Enter Passenger ID: ");
                    if (int.TryParse(Console.ReadLine(), out passengerId))
                    {
                        break;
                    }
                    Console.WriteLine("Invalid input. Please enter a valid integer for Passenger ID");
                }

                int flightNumber;
                Flight flight = null;
                while (flight == null)
                {
                    Console.Write("Enter Flight Number: ");
                    if (int.TryParse(Console.ReadLine(), out flightNumber))
                    {
                        flight = flightRepository.Find(f => f.FlightNumber == flightNumber);
                        if (flight == null)
                        {
                            Console.WriteLine("Flight not found. Please enter a valid Flight Number");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid integer for Flight Number");
                    }
                }

                FlightClass flightClass;
                while (true)
                {
                    Console.Write("Enter Flight Class (Economy, Business, FirstClass): ");
                    var flightClassInput = Console.ReadLine();
                    if (Enum.TryParse(flightClassInput, true, out flightClass))
                    {
                        break;
                    }
                    Console.WriteLine("Invalid input. Please enter a valid Flight Class (Economy, Business, FirstClass)");
                }

                var passenger = new Passenger { Id = passengerId }; 
                _passengerService.BookFlight(passenger, flight, flightClass);

                Console.WriteLine("Flight booked successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void ViewPersonalBookings()
        {
            try
            {
                Console.Write("Enter Passenger ID to view bookings: ");
                var passengerId = int.Parse(Console.ReadLine());

                var bookings = _passengerService.ViewPersonalBookings(passengerId);

                if (bookings.Count > 0)
                {
                    Console.WriteLine("Your bookings:");
                    foreach (var booking in bookings)
                    {
                        Console.WriteLine($"Booking ID: {booking.BookingId}, Flight ID: {booking.FlightId}, Class: {booking.Class}, Price: {booking.Price}");
                    }
                }
                else
                {
                    Console.WriteLine("No bookings found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void CancelBooking()
        {
            try
            {
                int passengerId;
                while (true)
                {
                    Console.Write("Enter Passenger ID: ");
                    if (int.TryParse(Console.ReadLine(), out passengerId))
                    {
                        break;
                    }
                    Console.WriteLine("Invalid input. Please enter a valid integer for Passenger ID.");
                }

                var bookings = _bookingService.GetBookingsForPassenger(passengerId);

                if (bookings.Count == 0)
                {
                    Console.WriteLine("No bookings found for the given Passenger ID.");
                    return;
                }

                Console.WriteLine("Your bookings:");
                foreach (var booking in bookings)
                {
                    Console.WriteLine($"Booking ID: {booking.BookingId}, Flight Number: {booking.FlightId}, Class: {booking.Class}, Price: {booking.Price}");
                }

                int bookingId = -1;
                Booking bookingToCancel = null;

                while (bookingToCancel == null)
                {
                    Console.Write("Enter Booking ID to cancel: ");
                    if (int.TryParse(Console.ReadLine(), out bookingId))
                    {
                        bookingToCancel = bookings.Find(b => b.BookingId == bookingId);
                        if (bookingToCancel == null)
                        {
                            Console.WriteLine("Booking not found. Please enter a valid Booking ID.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid integer for Booking ID.");
                    }
                }

                _passengerService.CancelBooking(bookingId);
                Console.WriteLine("Booking canceled successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void ModifyBooking()
        {
            try
            {
                int passengerId;
                while (true)
                {
                    Console.Write("Enter Passenger ID: ");
                    if (int.TryParse(Console.ReadLine(), out passengerId))
                    {
                        break;
                    }
                    Console.WriteLine("Invalid input. Please enter a valid integer for Passenger ID.");
                }

                var bookings = _bookingService.GetBookingsForPassenger(passengerId);

                if (bookings.Count == 0)
                {
                    Console.WriteLine("No bookings found for the given Passenger ID.");
                    return;
                }

                Console.WriteLine("Your bookings:");
                foreach (var booking in bookings)
                {
                    Console.WriteLine($"Booking ID: {booking.BookingId}, Flight Number: {booking.FlightId}, Class: {booking.Class}, Price: {booking.Price}");
                }

                int bookingId;
                Booking bookingToModify = null;
                while (bookingToModify == null)
                {
                    Console.Write("Enter Booking ID to modify: ");
                    if (int.TryParse(Console.ReadLine(), out bookingId))
                    {
                        bookingToModify = bookings.Find(b => b.BookingId == bookingId);
                        if (bookingToModify == null)
                        {
                            Console.WriteLine("Booking not found. Please enter a valid Booking ID.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid integer for Booking ID.");
                    }
                }

                int flightClassOption;
                FlightClass newFlightClass;
                while (true)
                {
                    Console.WriteLine("Enter new Flight Class (0 for Economy, 1 for Business, 2 for FirstClass): ");
                    if (int.TryParse(Console.ReadLine(), out flightClassOption))
                    {
                        if (flightClassOption >= 0 && flightClassOption <= 2)
                        {
                            newFlightClass = (FlightClass)flightClassOption;
                            bookingToModify.Class = newFlightClass;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid option. Please enter 0 for Economy, 1 for Business, or 2 for FirstClass.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid integer for Flight Class.");
                    }
                }

                _bookingService.ModifyBooking(bookingToModify);
                Console.WriteLine("Booking modified successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void SearchAvailableFlights()
        {
            try
            {
                Console.Write("Enter Departure Country: ");
                var departureCountry = Console.ReadLine();

                Console.Write("Enter Destination Country: ");
                var destinationCountry = Console.ReadLine();

                Console.Write("Enter Departure Date (yyyy-MM-dd): ");
                var departureDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Enter Departure Airport: ");
                var departureAirport = Console.ReadLine();

                Console.Write("Enter Arrival Airport: ");
                var arrivalAirport = Console.ReadLine();

                Console.Write("Enter Flight Class (Economy, Business, FirstClass): ");
                var flightClass = Enum.Parse<FlightClass>(Console.ReadLine(), true);

                var flights = _passengerService.SearchAvailableFlights(departureCountry, destinationCountry, departureDate, departureAirport, arrivalAirport, flightClass);

                if (flights.Count > 0)
                {
                    Console.WriteLine("Available flights:");
                    foreach (var flight in flights)
                    {
                        Console.WriteLine($"Flight Number: {flight.FlightNumber}, Departure: {flight.DepartureAirport} -> Arrival: {flight.ArrivalAirport}, Date: {flight.DepartureDate.ToShortDateString()}, Price: {flight.BasePrice}");
                    }
                }
                else
                {
                    Console.WriteLine("No flights found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void ViewAllAvailableFlights()
        {
            try
            {
                var flights = _flightRepository.GetAllFlights();

                if (flights.Count > 0)
                {
                    Console.WriteLine("All available flights:");
                    foreach (var flight in flights)
                    {
                        Console.WriteLine($"Flight Number: {flight.FlightNumber}, {flight.DepartureCountry}->{flight.DestinationCountry} ,Departure: {flight.DepartureAirport} -> Arrival: {flight.ArrivalAirport}, Date: {flight.DepartureDate.ToShortDateString()}, Price: {flight.BasePrice}");
                    }
                }
                else
                {
                    Console.WriteLine("No flights available.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void ImportFlightsFromCsv()
        {
            try
            {
                Console.Write("Enter CSV file path for flights import: ");
                var filePath = @"C:\Users\maham\Desktop\Airport Ticket Booking\Airport Ticket Booking\large_flights.CSV";

                var bookingManager = new BookingManager(_flightRepository, _validationService);
                bookingManager.ImportFlightsFromCsv(filePath);
                Console.WriteLine("Flights imported successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }
    }
}
