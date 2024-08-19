using Airport_Ticket_Booking.Interfaces;
using Airport_Ticket_Booking.Models;
using Airport_Ticket_Booking.Models.Enums;
using System.Collections.Generic;
using System.IO;

namespace Airport_Ticket_Booking.Services
{
    public class BookingRepository : IBookingRepository
    {
        private readonly string _filePath = @"C:\Users\maham\Desktop\Airport Ticket Booking\Airport Ticket Booking\bookings.csv";


        public List<Booking> GetAllBookings()
        {
            var bookings = new List<Booking>();

            if (!File.Exists(_filePath))
            {
                return bookings;
            }

            using (var reader = new StreamReader(_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    

                    if (!Enum.TryParse(parts[3], out FlightClass flightClass))
                    {
                        continue;
                    }

                    var booking = new Booking
                    {
                        BookingId = int.Parse(parts[0]),
                        FlightId = int.Parse(parts[1]),
                        PassengerId = int.Parse(parts[2]),
                        Class = flightClass,
                        Price = decimal.Parse(parts[4])
                    };
                    bookings.Add(booking);
                }
            }

            return bookings;
        }



        public void SaveBookings(List<Booking> bookings)
        {
            using (var writer = new StreamWriter(_filePath))
            {
                foreach (var booking in bookings)
                {
                    writer.WriteLine($"{booking.BookingId},{booking.FlightId},{booking.PassengerId},{booking.Class},{booking.Price}");
                }
            }
        }
    }
}
