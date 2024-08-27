using Airport_Ticket_Booking.Interfaces;
using Airport_Ticket_Booking.Models;
using Airport_Ticket_Booking.Models.Enums;
using Airport_Ticket_Booking.Services;
using FluentAssertions;
using Moq;

namespace Airport_Ticket_Booking.Tests
{
    public class BookingRepositoryTests
    {
        private readonly Mock<IFileStorage> _mockFileStorage;
        private readonly BookingRepository _repository;

        public BookingRepositoryTests()
        {
            _mockFileStorage = new Mock<IFileStorage>();
            _repository = new BookingRepository(_mockFileStorage.Object);
        }

        [Fact]
        public void GetAllBookings_FileExists_ReturnsListOfBookings()
        {
            //Arrange
            var expectedBookings = new List<Booking> {

              new Booking { BookingId = 1, FlightId = 100, PassengerId = 200, Class = FlightClass.Economy, Price = 0.01m },
              new Booking { BookingId = 2, FlightId = 101, PassengerId = 201, Class = FlightClass.Business, Price = 250.75m }

             };

            _mockFileStorage.Setup(x => x.ReadFromFile<Booking>(It.IsAny<string>()))
                            .Returns(expectedBookings);

            //Act
            var result = _repository.GetAllBookings();

            //Assert
            result.Should().NotBeNull().And.HaveCount(expectedBookings.Count).And.BeEquivalentTo(expectedBookings);
        }

        [Fact]
        public void GetAllBookings_FileNotFound_ReturnsEmptyList()
        {
            //Arrange 
            _mockFileStorage.Setup(x => x.ReadFromFile<Booking>(It.IsAny<string>())).Throws(new IOException("File not found"));

            //Act
            var result = _repository.GetAllBookings();


            //Assert
            result.Should().NotBeNull().And.BeEmpty();

        }

        [Fact]
        public void GetAllBookings_UnexpectedException_ReturnsEmptyList()
        {
            //Arrange
            _mockFileStorage.Setup(fs => fs.ReadFromFile<Booking>(It.IsAny<string>())).Throws(new Exception("Unexpected error"));

            //Act
            var result = _repository.GetAllBookings();

            //Assert
            result.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public void SaveBookings_ValidData_CallsWriteToFile()
        {
            //Arrange
            var Bookings = new List<Booking> {

              new Booking { BookingId = 1, FlightId = 100, PassengerId = 200, Class = FlightClass.Economy, Price = 0.01m },
              new Booking { BookingId = 2, FlightId = 101, PassengerId = 201, Class = FlightClass.Business, Price = 250.75m }

             };
            _mockFileStorage.Setup(x => x.WriteToFile(It.IsAny<List<Booking>>(), It.IsAny<string>()));

            //Act
            _repository.SaveBookings(Bookings);

            //Assert
            _mockFileStorage.Verify(x => x.WriteToFile(Bookings, It.IsAny<string>()), Times.Once);

        }

        [Fact]
        public void SaveBookings_IOException_LogsError()
        {
            // Arrange
            var Bookings = new List<Booking> {

              new Booking { BookingId = 1, FlightId = 100, PassengerId = 200, Class = FlightClass.Economy, Price = 0.01m },
              new Booking { BookingId = 2, FlightId = 101, PassengerId = 201, Class = FlightClass.Business, Price = 250.75m }

             };

            _mockFileStorage.Setup(fs => fs.WriteToFile(It.IsAny<List<Booking>>(), It.IsAny<string>())).Throws(new IOException("Unable to write to file"));
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act
            _repository.SaveBookings(Bookings);

            // Assert
            var output = stringWriter.ToString();
            output.Should().Contain("An error occurred while writing to the bookings file:");

        }

    }
}