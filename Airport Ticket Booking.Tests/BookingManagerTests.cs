using Airport_Ticket_Booking.Interfaces;
using Airport_Ticket_Booking.Models;
using Airport_Ticket_Booking.Record;
using Airport_Ticket_Booking.Services;
using FluentAssertions;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace Airport_Ticket_Booking.Tests
{

    public class BookingManagerTests
    {
        private readonly Mock<IFlightRepository> _mockFlightRepository;
        private readonly Mock<IValidationService> _mockValidationService;
        private readonly BookingManager _bookingManager;

        public BookingManagerTests()
        {
            _mockFlightRepository = new Mock<IFlightRepository>();
            _mockValidationService = new Mock<IValidationService>();
            _bookingManager = new BookingManager(_mockFlightRepository.Object, _mockValidationService.Object);
        }

        [Fact]
        public void FilterBookings_ValidCriteria_ReturnsFilteredFlights()
        {
            // Arrange
            var criteria = new FilterCriteria
            {
                DepartureCountry = "JORDAN",
                DestinationCountry = "UK",
                DepartureDate = new DateTime(2024, 11, 27),
                DepartureAirport = "AAC",
                ArrivalAirport = "LHR",
                Price = 100m
            };

            var flights = new List<Flight>
            {
                new Flight { FlightNumber = 1,DepartureCountry = "JORDAN",DestinationCountry = "UK", DepartureDate = new DateTime(2024, 11, 27),DepartureAirport = "AAC",ArrivalAirport = "LHR",BasePrice = 100m},
                new Flight { FlightNumber = 2,DepartureCountry = "JORDAN",DestinationCountry = "UK",DepartureDate = new DateTime(2024, 12, 30),DepartureAirport = "AAC",ArrivalAirport = "LHR",BasePrice = 150m}
            };

            _mockFlightRepository.Setup(r => r.GetAllFlights()).Returns(flights);

            // Act
            var result = _bookingManager.FilterBookings(criteria);

            // Assert
            result.Should().HaveCount(1);
            result.First().Should().BeEquivalentTo(flights.First());
        }

        [Fact]
        public void ImportFlightsFromCsv_ValidFile_SuccessfulImport()
        {
            //Arrange
            var filePath = "flights.csv";
            var fileContent = "FlightNumber,DepartureCountry,DestinationCountry,DepartureDate,DepartureAirport,ArrivalAirport,BasePrice\n" + "8705,USA,UK,2025-01-18,SEA,LHR,831.56\n";

            File.WriteAllText(filePath, fileContent);
            var flights = new List<Flight> { new Flight { FlightNumber = 8705, DepartureCountry = "USA", DestinationCountry = "UK", DepartureDate = new DateTime(2025, 01, 18), DepartureAirport = "SEA", ArrivalAirport = "LHR", BasePrice = 831.56m } };

            _mockValidationService.Setup(x => x.ValidateFlight(It.IsAny<Flight>())).Returns(new List<ValidationResult>());
            _mockFlightRepository.Setup(x => x.SaveFlights(It.IsAny<List<Flight>>()));

            //Act
            _bookingManager.ImportFlightsFromCsv(filePath);

            //Assert
            _mockFlightRepository.Verify(r => r.SaveFlights(It.IsAny<List<Flight>>()), Times.Once);
        }

    }
}