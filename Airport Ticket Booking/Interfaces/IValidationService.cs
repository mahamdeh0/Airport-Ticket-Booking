using Airport_Ticket_Booking.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.Interfaces
{
    public interface IValidationService
    {
        List<ValidationResult> ValidateFlight(Flight flight);
        Dictionary<string, string> GetValidationConstraints();
    }

}
