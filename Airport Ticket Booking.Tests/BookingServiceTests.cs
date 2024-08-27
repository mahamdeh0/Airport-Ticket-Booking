using Airport_Ticket_Booking.Interfaces;
using Airport_Ticket_Booking.Models.Enums;
using Airport_Ticket_Booking.Models;
using Moq;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.IO;
using FluentAssertions;

namespace Airport_Ticket_Booking.Tests
{
    public class BookingServiceTests
    {
        private readonly Mock<IBookingRepository> _mockBookingRepository;
        private readonly BookingService _bookingService;

        public BookingServiceTests()
        {
            _mockBookingRepository = new Mock<IBookingRepository>();
            _bookingService = new BookingService(_mockBookingRepository.Object);
        }

        [Fact]
        public void BookFlight_ValidBooking_AddsBooking()
        {
            //Arrange
            var booking = new Booking { BookingId = 1, FlightId = 100, PassengerId = 200, Class = FlightClass.Economy, Price = 0.01m };
            var bookings = new List<Booking>();
            _mockBookingRepository.Setup(x => x.GetAllBookings()).Returns(bookings);
            _mockBookingRepository.Setup(x => x.SaveBookings(It.IsAny<List<Booking>>()));

            //Act
            _bookingService.BookFlight(booking);

            //Assert
            _mockBookingRepository.Verify(x => x.SaveBookings(It.Is<List<Booking>>(y => y.Contains(booking))), Times.Once);

        }

        [Fact]
        public void BookFlight_ExceptionThrown_ReturnExceptionMessage()
        {
            //Arrange
            var booking = new Booking { BookingId = 1, FlightId = 100, PassengerId = 200, Class = FlightClass.Economy, Price = 100m };
            _mockBookingRepository.Setup(x => x.GetAllBookings()).Throws(new Exception());
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            //Act
            _bookingService.BookFlight(booking);

            //Assert
            var output = stringWriter.ToString();
            output.Should().Contain("Error booking flight:");

        }

        [Fact]
        public void CancelBooking_ExistingBooking_RemovesBooking()
        {
            var booking = new Booking { BookingId = 1, FlightId = 100, PassengerId = 200, Class = FlightClass.Economy, Price = 100m };
            var bookings = new List<Booking> { booking };
            _mockBookingRepository.Setup(r => r.GetAllBookings()).Returns(bookings);
            _mockBookingRepository.Setup(r => r.SaveBookings(It.IsAny<List<Booking>>()));

            //Act
            _bookingService.CancelBooking(1);

            //Assert
            _mockBookingRepository.Verify(x => x.SaveBookings(It.Is<List<Booking>>(y => !y.Contains(booking))), Times.Once);

        }

        [Fact]
        public void CancelBooking_BookingNotFound_ReturnMessage()
        {
            //Arrange
            var bookingId = 99; 
            var bookings = new List<Booking>(); 
            _mockBookingRepository.Setup(x => x.GetAllBookings()).Returns(bookings);
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            //Act
            _bookingService.CancelBooking(bookingId);

            //Assert
            var output = stringWriter.ToString();
            output.Should().Contain("Booking not found");
            
        }

        [Fact]
        public void ModifyBooking_ExistingBooking_ModifiesBooking()
        {
            //Arrange
            var Booking = new Booking { BookingId = 1, FlightId = 100, PassengerId = 200, Class = FlightClass.Economy, Price = 100.00m };
            var modifiedBooking = new Booking { BookingId = 1, FlightId = 100, PassengerId = 200, Class = FlightClass.Business, Price = 100.00m };
            var bookings = new List<Booking> { Booking };
            _mockBookingRepository.Setup(x => x.GetAllBookings()).Returns(bookings);
            _mockBookingRepository.Setup(x => x.SaveBookings(It.IsAny<List<Booking>>()));

            //Act
            _bookingService.ModifyBooking(modifiedBooking);

            //Assert
            _mockBookingRepository.Verify(x => x.SaveBookings(It.Is<List<Booking>>(y => y.Contains(modifiedBooking) && !y.Contains(Booking))), Times.Once);
        }

        [Fact]
        public void ModifyBooking_BookingNotFound_LogsNotFoundMessage()
        {
            //Arrange
            var modifiedBooking = new Booking { BookingId = 1, FlightId = 100, PassengerId = 200, Class = FlightClass.Business, Price = 100.00m };
            var bookings = new List<Booking>();
            _mockBookingRepository.Setup(x => x.GetAllBookings()).Returns(bookings);
            _mockBookingRepository.Setup(x => x.SaveBookings(It.IsAny<List<Booking>>()));
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            //Act
            _bookingService.ModifyBooking(modifiedBooking);

            //Assert
            var output = stringWriter.ToString();
            output.Should().Contain("Booking not found.");
            
        }

        [Fact]
        public void GetBookingsForPassenger_ExistingBookings_ReturnsBookingsForPassenger()
        {
            //Arrange
            var bookings = new List<Booking>
            {
            new Booking { BookingId = 1, FlightId = 100, PassengerId = 1, Class = FlightClass.Economy, Price = 100.00m },
            new Booking { BookingId = 2, FlightId = 101, PassengerId = 1, Class = FlightClass.Business, Price = 200.00m },
            new Booking { BookingId = 3, FlightId = 102, PassengerId = 99, Class = FlightClass.Economy, Price = 150.00m }
            };
            var expectedBookings = bookings.Where(b => b.PassengerId == 1).ToList();
            _mockBookingRepository.Setup(x => x.GetAllBookings()).Returns(bookings);

            //Act
            var result = _bookingService.GetBookingsForPassenger(1);

            //Assert
            result.Should().NotBeNull().And.HaveCount(expectedBookings.Count).And.Equal(expectedBookings);
        }

    }
}
