using Airport_Ticket_Booking.Interfaces;
using System.Text.Json;

namespace Airport_Ticket_Booking.Storage
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
