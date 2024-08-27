using Airport_Ticket_Booking.Interfaces;
using Airport_Ticket_Booking.Models;
using Airport_Ticket_Booking.Models.Enums;
using Airport_Ticket_Booking.Services;
using FluentAssertions;
using Moq;

namespace Airport_Ticket_Booking.Tests
{
    public class PassengerServiceTests
    {
        private readonly Mock<IBookingService> _mockBookingService;
        private readonly Mock<IFlightRepository> _mockFlightRepository;
        private readonly PassengerService _passengerService;

        public PassengerServiceTests()
        {
            _mockBookingService = new Mock<IBookingService>();
            _mockFlightRepository = new Mock<IFlightRepository>();
            _passengerService = new PassengerService(_mockBookingService.Object, _mockFlightRepository.Object);
        }

        [Fact]
        public void BookFlight_ValidInput_CreatesBooking()
        {
            //Arrange
            var passenger = new Passenger { Id = 1 };
            var flight = new Flight { FlightNumber = 123, DepartureCountry = "France", DestinationCountry = "Canada", DepartureDate = new DateTime(2024, 11, 1), DepartureAirport = "JFK", ArrivalAirport = "YYZ", BasePrice = 100.00m };
            var flightClass = FlightClass.Business;

            //Act
            _passengerService.BookFlight(passenger, flight, flightClass);

            //Assert
            _mockBookingService.Verify(x => x.BookFlight(It.Is<Booking>(y => y.FlightId == flight.FlightNumber && y.PassengerId == passenger.Id && y.Class == flightClass && y.Price == 150m)), Times.Once);
        }

        [Fact]
        public void CancelBooking_ValidBookingId_CallsCancelBooking()
        {
            //Arrange
            int bookingId = 1;

            //Act
            _passengerService.CancelBooking(bookingId);

            //Assert
            _mockBookingService.Verify(x => x.CancelBooking(bookingId), Times.Once);
        }

        [Fact]
        public void ModifyBooking_ValidBooking_CallsModifyBooking()
        {
            //Arrange
            var booking = new Booking { BookingId = 1 };

            //Act
            _passengerService.ModifyBooking(booking);

            //Assert
            _mockBookingService.Verify(x => x.ModifyBooking(booking), Times.Once);
        }

        [Fact]
        public void SearchAvailableFlights_ValidCriteria_ReturnsFlights()
        {
            //Arrange
            var criteria = new FlightSearchCriteria(DepartureCountry: "France", DestinationCountry: "Canada", DepartureDate: new DateTime(2024, 11, 17), DepartureAirport: "RFA", ArrivalAirport: "LHR", FlightClass: FlightClass.Economy);
            var flights = new List<Flight>
            {
               new Flight { FlightNumber = 123, DepartureCountry = "France", DestinationCountry = "Canada", DepartureDate = new DateTime(2024, 11, 17), DepartureAirport = "RFA", ArrivalAirport = "LHR", BasePrice = 100.00m }
            };

            _mockFlightRepository.Setup(x => x.GetAllFlights()).Returns(flights);

            //Act
            var result = _passengerService.SearchAvailableFlights(criteria);

            //Assert
            result.Should().HaveCount(1);
            result.First().Should().BeEquivalentTo(flights.First());
        }

        [Fact]
        public void ViewPersonalBookings_ValidPassengerId_ReturnsBookings()
        {
            //Arrange
            int passengerId = 1;
            var bookings = new List<Booking> { new Booking { PassengerId = passengerId } };

            _mockBookingService.Setup(x => x.GetBookingsForPassenger(passengerId)).Returns(bookings);

            //Act
            var result = _passengerService.ViewPersonalBookings(passengerId);

            //Assert
            result.Should().HaveCount(1);
            result.First().PassengerId.Should().Be(passengerId);
        }

    }
}
