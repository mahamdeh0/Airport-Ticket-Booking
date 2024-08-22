using Airport_Ticket_Booking.Interfaces;
using Airport_Ticket_Booking.Models;
using Airport_Ticket_Booking.Models.Enums;
using Airport_Ticket_Booking.Record;
using Airport_Ticket_Booking.Services;

namespace Airport_Ticket_Booking.Utilities
{
    public class Menu
    {
        private readonly IBookingService _bookingService;
        private readonly IFlightRepository _flightRepository;
        private readonly IPassengerService _passengerService;
        private readonly IValidationService _validationService;
        private readonly IBookingManager _bookingManager;


        public Menu(IBookingService bookingService, IFlightRepository flightRepository, IPassengerService passengerService, IValidationService validationService, IBookingManager bookingManager)
        {
            _bookingService = bookingService;
            _flightRepository = flightRepository;
            _passengerService = passengerService;
            _validationService = validationService;
            _bookingManager = bookingManager;
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
                Console.WriteLine("7. Filter Bookings");
                Console.WriteLine("8. Import Flights from CSV");
                Console.WriteLine("9. Exit");
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
                        FilterBookings();
                        break;
                    case "8":
                        ImportFlightsFromCsv();
                        break;
                    case "9":
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
                if (!DateTime.TryParse(Console.ReadLine(), out var departureDate))
                {
                    Console.WriteLine("Invalid date format.");
                    return;
                }

                Console.Write("Enter Departure Airport: ");
                var departureAirport = Console.ReadLine();

                Console.Write("Enter Arrival Airport: ");
                var arrivalAirport = Console.ReadLine();

                Console.Write("Enter Flight Class (Economy, Business, FirstClass): ");
                if (!Enum.TryParse<FlightClass>(Console.ReadLine(), true, out var flightClass))
                {
                    Console.WriteLine("Invalid flight class.");
                    return;
                }

                var criteria = new FlightSearchCriteria(
                    departureCountry,
                    destinationCountry,
                    departureDate,
                    departureAirport,
                    arrivalAirport,
                    flightClass
                );

                var flights = _passengerService.SearchAvailableFlights(criteria);

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

        private void FilterBookings()
        {
            try
            {
                Console.WriteLine("Let's filter the bookings according to your preferences.");

                Console.Write("Do you want to filter by Maximum Price? (yes/no): ");
                bool filterByPrice = Console.ReadLine().Trim().ToLower() == "yes";
                decimal? price = null;
                if (filterByPrice)
                {
                    Console.Write("Enter Maximum Price: ");
                    var priceInput = Console.ReadLine();
                    price = string.IsNullOrEmpty(priceInput) ? (decimal?)null : decimal.Parse(priceInput);
                }

                Console.Write("Do you want to filter by Departure Country? (yes/no): ");
                bool filterByDepartureCountry = Console.ReadLine().Trim().ToLower() == "yes";
                string departureCountry = null;
                if (filterByDepartureCountry)
                {
                    Console.Write("Enter Departure Country: ");
                    departureCountry = Console.ReadLine();
                }

                Console.Write("Do you want to filter by Destination Country? (yes/no): ");
                bool filterByDestinationCountry = Console.ReadLine().Trim().ToLower() == "yes";
                string destinationCountry = null;
                if (filterByDestinationCountry)
                {
                    Console.Write("Enter Destination Country: ");
                    destinationCountry = Console.ReadLine();
                }

                Console.Write("Do you want to filter by Departure Date? (yes/no): ");
                bool filterByDepartureDate = Console.ReadLine().Trim().ToLower() == "yes";
                DateTime? departureDate = null;
                if (filterByDepartureDate)
                {
                    Console.Write("Enter Departure Date (yyyy-MM-dd): ");
                    var departureDateInput = Console.ReadLine();
                    departureDate = string.IsNullOrEmpty(departureDateInput) ? (DateTime?)null : DateTime.Parse(departureDateInput);
                }

                Console.Write("Do you want to filter by Departure Airport? (yes/no): ");
                bool filterByDepartureAirport = Console.ReadLine().Trim().ToLower() == "yes";
                string departureAirport = null;
                if (filterByDepartureAirport)
                {
                    Console.Write("Enter Departure Airport: ");
                    departureAirport = Console.ReadLine();
                }

                Console.Write("Do you want to filter by Arrival Airport? (yes/no): ");
                bool filterByArrivalAirport = Console.ReadLine().Trim().ToLower() == "yes";
                string arrivalAirport = null;
                if (filterByArrivalAirport)
                {
                    Console.Write("Enter Arrival Airport: ");
                    arrivalAirport = Console.ReadLine();
                }

                var criteria = new FilterCriteria(
                    Price: price,
                    DepartureCountry: departureCountry,
                    DestinationCountry: destinationCountry,
                    DepartureDate: departureDate,
                    DepartureAirport: departureAirport,
                    ArrivalAirport: arrivalAirport
                );

                var flights = _bookingManager.FilterBookings(criteria);

                if (flights.Count > 0)
                {
                    Console.WriteLine("Filtered flights:");
                    foreach (var flight in flights)
                    {
                        Console.WriteLine($"Flight Number: {flight.FlightNumber}, {flight.DepartureCountry} -> {flight.DestinationCountry}, Departure: {flight.DepartureAirport} -> Arrival: {flight.ArrivalAirport}, Date: {flight.DepartureDate.ToShortDateString()}, Price: {flight.BasePrice}");
                    }
                }
                else
                {
                    Console.WriteLine("No flights found matching the criteria.");
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
