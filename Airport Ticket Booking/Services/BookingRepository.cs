using Airport_Ticket_Booking.Interfaces;
using Airport_Ticket_Booking.Models;
using Airport_Ticket_Booking.Models.Enums;
using System.Collections.Generic;
using System.IO;

namespace Airport_Ticket_Booking.Services
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IFileStorage _fileStorage;
        private readonly string _filePath = @"C:\Users\maham\Desktop\Airport Ticket Booking\Airport Ticket Booking\bookings.csv";

        public BookingRepository(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }

        public List<Booking> GetAllBookings()
        {
            return _fileStorage.ReadFromFile<Booking>(_filePath);
        }

        public void SaveBookings(List<Booking> bookings)
        {
            _fileStorage.WriteToFile(bookings, _filePath);
        }
    }
}
