using Airport_Ticket_Booking.Interfaces;
using Airport_Ticket_Booking.Services;
using Airport_Ticket_Booking.Storage;
using Airport_Ticket_Booking.Utilities;

class Program
{
    static void Main(string[] args)
    {
        IFileStorage fileStorage = new FileStorage();
        IFlightRepository flightRepository = new FlightRepository(fileStorage);
        IBookingRepository bookingRepository = new BookingRepository(fileStorage);
        IBookingService bookingService = new BookingService(bookingRepository);
        IValidationService validationService = new ValidationService();
        IBookingManager bookingManager = new BookingManager(flightRepository, validationService);
        IPassengerService passengerService = new PassengerService(bookingService, flightRepository);

        var menu = new Menu(bookingService, flightRepository, passengerService, validationService, bookingManager);
        menu.ShowMenu();
    }
}