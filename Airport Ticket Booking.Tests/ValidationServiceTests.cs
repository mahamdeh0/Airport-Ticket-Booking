using Airport_Ticket_Booking.Models;
using Airport_Ticket_Booking.Services;
using FluentAssertions;

namespace Airport_Ticket_Booking.Tests
{
    public class ValidationServiceTests
    {
        [Fact]
        public void ValidateFlight_ValidFlight_ReturnsNoValidationResults()
        {
            //Arrange
            var flight = new Flight { FlightNumber = 123, DepartureCountry = "France", DestinationCountry = "Canada", DepartureDate = new DateTime(2024, 11, 1), DepartureAirport = "JFK", ArrivalAirport = "YYZ", BasePrice = 100.00m };
            var validationService = new ValidationService();

            //Act
            var results = validationService.ValidateFlight(flight);

            //Assert
            results.Should().BeEmpty();
        }

        [Fact]
        public void ValidateFlight_InvalidFlight_ReturnsValidationResults()
        {
            //Arrange
            var flight = new Flight
            {
                FlightNumber = -1,
                DepartureCountry = new string('a', 200),
                DestinationCountry = new string('a', 200),
                DepartureDate = new DateTime(2000, 11, 1),
                DepartureAirport = new string('a', 200),
                ArrivalAirport = new string('a', 200),
                BasePrice = -100.00m
            };
            var validationService = new ValidationService();

            //Act
            var results = validationService.ValidateFlight(flight);

            //Assert
            results.Should().NotBeEmpty();
            results.Should().Contain(x => x.ErrorMessage == "FlightNumber must be a positive integer");
            results.Should().Contain(x => x.ErrorMessage == "DepartureCountry cannot exceed 100 characters");
            results.Should().Contain(x => x.ErrorMessage == "DestinationCountry cannot exceed 100 characters");
            results.Should().Contain(x => x.ErrorMessage == "DepartureDate cannot be in the past");
            results.Should().Contain(x => x.ErrorMessage == "DepartureAirport cannot exceed 10 characters");
            results.Should().Contain(x => x.ErrorMessage == "ArrivalAirport cannot exceed 10 characters");
            results.Should().Contain(x => x.ErrorMessage == "BasePrice must be greater than zero");
        }

        [Fact]
        public void GetValidationConstraints_ReturnsCorrectConstraints()
        {
            //Arrange
            var validationService = new ValidationService();
            var expectedConstraints = new Dictionary<string, string>
            {
                 { "FlightNumber", "Type: Integer; Constraint: Required; Positive integer" },
                 { "DepartureCountry", "Type: String; Constraint: Required; Max length 100 characters" },
                 { "DestinationCountry", "Type: String; Constraint: Required; Max length 100 characters" },
                 { "DepartureDate", "Type: DateTime; Constraint: Required; Must be today's date or a future date" },
                 { "DepartureAirport", "Type: String; Constraint: Required; Max length 10 characters" },
                 { "ArrivalAirport", "Type: String; Constraint: Required; Max length 10 characters" },
                 { "BasePrice", "Type: Decimal; Constraint: Required; Must be greater than zero" }
            };

            //Act
            var constraints = validationService.GetValidationConstraints();

            //Assert
            constraints.Should().Equal(expectedConstraints);
        }
    }
}
