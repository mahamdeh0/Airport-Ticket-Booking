using Airport_Ticket_Booking.Interfaces;
using Airport_Ticket_Booking.Models;
using System;
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
            try
            {
                return _fileStorage.ReadFromFile<Booking>(_filePath);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while reading the bookings file: {ex.Message}");
                return new List<Booking>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred while retrieving bookings: {ex.Message}");
                return new List<Booking>();
            }
        }

        public void SaveBookings(List<Booking> bookings)
        {
            try
            {
                _fileStorage.WriteToFile(bookings, _filePath);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while writing to the bookings file: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred while saving bookings: {ex.Message}");
            }
        }
    }
}
