using Airport_Ticket_Booking.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Airport_Ticket_Booking.Models
{
    public class Booking
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "BookingId must be a positive integer.")]
        public int BookingId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "FlightId must be a positive integer.")]
        public int FlightId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "PassengerId must be a positive integer.")]
        public int PassengerId { get; set; }

        [Required]
        [EnumDataType(typeof(FlightClass), ErrorMessage = "Invalid flight class.")]
        public FlightClass Class { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
    }
}
