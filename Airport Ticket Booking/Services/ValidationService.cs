using Airport_Ticket_Booking.Interfaces;
using Airport_Ticket_Booking.Models;
using System.ComponentModel.DataAnnotations;

namespace Airport_Ticket_Booking.Services
{

    public class ValidationService : IValidationService
    {
        public List<ValidationResult> ValidateFlight(Flight flight)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(flight);
            Validator.TryValidateObject(flight, context, results, validateAllProperties: true);
            return results;
        }

        public Dictionary<string, string> GetValidationConstraints()
        {
            return new Dictionary<string, string>
            {
                 { "FlightNumber", "Type: Integer; Constraint: Required; Positive integer" },
                 { "DepartureCountry", "Type: String; Constraint: Required; Max length 100 characters" },
                 { "DestinationCountry", "Type: String; Constraint: Required; Max length 100 characters" },
                 { "DepartureDate", "Type: DateTime; Constraint: Required; Must be today's date or a future date" },
                 { "DepartureAirport", "Type: String; Constraint: Required; Max length 10 characters" },
                 { "ArrivalAirport", "Type: String; Constraint: Required; Max length 10 characters" },
                 { "BasePrice", "Type: Decimal; Constraint: Required; Must be greater than zero" }
            };
        }
    }
}
