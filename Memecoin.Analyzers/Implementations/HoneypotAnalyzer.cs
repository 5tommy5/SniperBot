﻿using System.Text.Json;
using Core;
using Memecoin.Analyzers.Models;

namespace Memecoin.Analyzers.Implementations
{
    public class HoneypotAnalyzer : ITokenAnalyzer
    {
        private readonly HttpClient _client;
        public HoneypotAnalyzer(HttpClient client)
        {
            _client = client;
        }

        public ChainEnum[]? SupportedChains => new [] { ChainEnum.Bsc };

        public async Task<AnalysisResult> Analyze(TokenInfo info)
        {
            if (info.Chain != "bsc")
                return AnalysisResult.SafeResult();
            
            var result = await CheckTokenAsync(info.Address);

            if (result == null)
                return AnalysisResult.UnSafeResult("Не удалось получить ответ от honeypot.is");

            if (result.HoneypotResult?.IsHoneypot == true)
                return AnalysisResult.UnSafeResult("Токен — honeypot");

            if (result.Summary?.RiskLevel > 1)
                return AnalysisResult.UnSafeResult($"Риск: {result.Summary.Risk}");

            if (result.SimulationResult?.SellTax > 15 || result.SimulationResult?.BuyTax > 15)
                return AnalysisResult.UnSafeResult($"Высокий налог: Buy {result.SimulationResult.BuyTax}%, Sell {result.SimulationResult.SellTax}%");

            return AnalysisResult.SafeResult();
        }

        private async Task<HoneypotResponse?> CheckTokenAsync(string tokenAddress)
        {
            var url = $"https://api.honeypot.is/v2/IsHoneypot?address={tokenAddress}";
            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<HoneypotResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
