using Airport_Ticket_Booking.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.Services
{
    public class FileStorage : IFileStorage
    {
        public void WriteToFile<T>(List<T> items, string filePath)
        {
            var json = JsonSerializer.Serialize(items);
            File.WriteAllText(filePath, json);
        }

        public List<T> ReadFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<T>();
            }

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(json);
        }
    }
}
