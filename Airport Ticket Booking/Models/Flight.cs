using System;
using System.ComponentModel.DataAnnotations;

namespace Airport_Ticket_Booking.Models
{
    public class Flight
    {
        [Required(ErrorMessage = "FlightNumber is required")]
        [Range(1, int.MaxValue, ErrorMessage = "FlightNumber must be a positive integer")]
        public int FlightNumber { get; set; }

        [Required(ErrorMessage = "DepartureCountry is required")]
        [StringLength(100, ErrorMessage = "DepartureCountry cannot exceed 100 characters")]
        public string DepartureCountry { get; set; }

        [Required(ErrorMessage = "DestinationCountry is required")]
        [StringLength(100, ErrorMessage = "DestinationCountry cannot exceed 100 characters")]
        public string DestinationCountry { get; set; }

        [Required(ErrorMessage = "DepartureDate is required")]
        [DataType(DataType.Date)]
        [DateInFuture(ErrorMessage = "DepartureDate cannot be in the past")]
        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "DepartureAirport is required.")]
        [StringLength(10, ErrorMessage = "DepartureAirport cannot exceed 10 characters")]
        public string DepartureAirport { get; set; }

        [Required(ErrorMessage = "ArrivalAirport is required.")]
        [StringLength(10, ErrorMessage = "ArrivalAirport cannot exceed 10 characters")]
        public string ArrivalAirport { get; set; }

        [Required(ErrorMessage = "BasePrice is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "BasePrice must be greater than zero")]
        public decimal BasePrice { get; set; }
    }

    public class DateInFutureAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date >= DateTime.Today;
            }
            return false;
        }
    }
}
