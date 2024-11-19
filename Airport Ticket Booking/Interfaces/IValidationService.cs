using Airport_Ticket_Booking.Models;
using System.ComponentModel.DataAnnotations;

namespace Airport_Ticket_Booking.Interfaces
{
    public interface IValidationService
    {
        List<ValidationResult> ValidateFlight(Flight flight);
        Dictionary<string, string> GetValidationConstraints();
    }

}
