using SniperBot.Monitor.Models;
using System.Text.Json;

namespace SniperBot.Monitor.Services
{
    internal static class SnipedLogs
    {
        internal static void SaveSnipedInfo(SnipedPairInfo info)
        {
            var filePath = "sniped_pairs.json";
            List<SnipedPairInfo> all = new();

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    all = JsonSerializer.Deserialize<List<SnipedPairInfo>>(json) ?? new();
                }
            }

            all.Add(info);

            var newJson = JsonSerializer.Serialize(all, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, newJson);
        }
    }
}
