using SniperBot.Monitor.Models;
using System.Text.Json;

namespace SniperBot.Monitor.Services
{
    public class HoneypotCheckService
    {
        public HoneypotCheckService()
        {
        }

        public async Task<HoneypotResponse?> CheckTokenAsync(string tokenAddress)
        {
            using var httpClient = new HttpClient(); 
            var url = $"https://api.honeypot.is/v2/IsHoneypot?address={tokenAddress}";
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<HoneypotResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<(bool,string)> AnalyzeAsync(string tokenAddress)
        {
            var result = await CheckTokenAsync(tokenAddress);

            if (result == null)
                return (false, "❌ Не удалось получить ответ от honeypot.is");

            if (result.HoneypotResult?.IsHoneypot == true)
                return (false, "⚠️ Токен — honeypot");

            if (result.Summary?.RiskLevel > 1)
                return (false, $"⚠️ Риск: {result.Summary.Risk}");

            if (result.SimulationResult?.SellTax > 15 || result.SimulationResult?.BuyTax > 15)
                return (false, $"⚠️ Высокий налог: Buy {result.SimulationResult.BuyTax}%, Sell {result.SimulationResult.SellTax}%");

            return (true, $"✅ OK: Honeypot={result.HoneypotResult?.IsHoneypot}, Risk={result.Summary?.Risk}");
        }
    }
}
