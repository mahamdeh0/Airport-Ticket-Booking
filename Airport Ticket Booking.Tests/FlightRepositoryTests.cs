using Airport_Ticket_Booking.Interfaces;
using Airport_Ticket_Booking.Models;
using Airport_Ticket_Booking.Services;
using FluentAssertions;
using Moq;

namespace Airport_Ticket_Booking.Tests
{
    public class FlightRepositoryTests
    {
        private readonly Mock<IFileStorage> _mockFileStorage;
        private readonly FlightRepository _repository;

        public FlightRepositoryTests()
        {
            _mockFileStorage = new Mock<IFileStorage>();
            _repository = new FlightRepository(_mockFileStorage.Object);

        }

        [Fact]
        public void GetAllFlights_FileExists_ReturnsListOfFlights()
        {
            //Arrange
            var expectedFlights = new List<Flight>
            {
                  new Flight { FlightNumber = 123, DepartureCountry = "France", DestinationCountry = "Canada", DepartureDate = new DateTime(2024, 11, 1), DepartureAirport = "JFK", ArrivalAirport = "YYZ", BasePrice = 100.00m },
                  new Flight { FlightNumber = 456, DepartureCountry = "Canada", DestinationCountry = "UK", DepartureDate = new DateTime(2024, 10, 2), DepartureAirport = "LAX", ArrivalAirport = "LGW", BasePrice = 200.00m },
                  new Flight { FlightNumber = 789, DepartureCountry = "USA", DestinationCountry = "France", DepartureDate = new DateTime(2025, 12, 3), DepartureAirport = "ORD", ArrivalAirport = "CDG", BasePrice = 300.00m }
            };

            _mockFileStorage.Setup(x => x.ReadFromFile<Flight>(It.IsAny<string>())).Returns(expectedFlights);

            //Act
            var result = _repository.GetAllFlights();

            //Assert
            result.Should().NotBeNull().And.HaveCount(expectedFlights.Count).And.BeEquivalentTo(expectedFlights);
        }

        [Fact]
        public void GetAllFlights_FileNotFound_ReturnsEmptyList()
        {
            //Arrange 
            _mockFileStorage.Setup(x => x.ReadFromFile<Flight>(It.IsAny<string>())).Throws(new IOException("File not found"));

            //Act
            var result = _repository.GetAllFlights();

            //Assert
            result.Should().NotBeNull().And.BeEmpty();

        }

        [Fact]
        public void GetAllFlights_UnexpectedException_ReturnsEmptyList()
        {
            //Arrange
            _mockFileStorage.Setup(x => x.ReadFromFile<Flight>(It.IsAny<string>())).Throws(new Exception("Unexpected error"));

            //Act
            var result = _repository.GetAllFlights();

            //Assert
            result.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public void SaveFlights_ValidData_CallsWriteToFile()
        {
            //Arrange
            var Flights = new List<Flight>
            {
                  new Flight { FlightNumber = 123, DepartureCountry = "France", DestinationCountry = "Canada", DepartureDate = new DateTime(2024, 11, 1), DepartureAirport = "JFK", ArrivalAirport = "YYZ", BasePrice = 100.00m },
                  new Flight { FlightNumber = 456, DepartureCountry = "Canada", DestinationCountry = "UK", DepartureDate = new DateTime(2024, 10, 2), DepartureAirport = "LAX", ArrivalAirport = "LGW", BasePrice = 200.00m },
                  new Flight { FlightNumber = 789, DepartureCountry = "USA", DestinationCountry = "France", DepartureDate = new DateTime(2025, 12, 3), DepartureAirport = "ORD", ArrivalAirport = "CDG", BasePrice = 300.00m }
            };

            //Act
            _repository.SaveFlights(Flights);

            //Assert
            _mockFileStorage.Verify(x => x.WriteToFile(Flights, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void SaveFlights_IOException_ReturnsIOException()
        {
            // Arrange
            var Flights = new List<Flight>
            {
                  new Flight { FlightNumber = 123, DepartureCountry = "France", DestinationCountry = "Canada", DepartureDate = new DateTime(2024, 11, 1), DepartureAirport = "JFK", ArrivalAirport = "YYZ", BasePrice = 100.00m },
                  new Flight { FlightNumber = 456, DepartureCountry = "Canada", DestinationCountry = "UK", DepartureDate = new DateTime(2024, 10, 2), DepartureAirport = "LAX", ArrivalAirport = "LGW", BasePrice = 200.00m },
                  new Flight { FlightNumber = 789, DepartureCountry = "USA", DestinationCountry = "France", DepartureDate = new DateTime(2025, 12, 3), DepartureAirport = "ORD", ArrivalAirport = "CDG", BasePrice = 300.00m }
            };

            _mockFileStorage.Setup(x => x.WriteToFile(It.IsAny<List<Flight>>(), It.IsAny<string>())).Throws(new IOException());
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act
            _repository.SaveFlights(Flights);

            // Assert
            var output = stringWriter.ToString();
            output.Should().Contain("An error occurred while writing to the flights file:");
        }
    }
}
