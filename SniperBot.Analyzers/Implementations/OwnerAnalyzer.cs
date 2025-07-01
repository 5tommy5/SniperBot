using Microsoft.Extensions.Logging;
using SniperBot.Analyzers.Models;


namespace SniperBot.Analyzers.Implementations
{
    internal class OwnerAnalyzer : ITokenAnalyzer
    {
        private const string NO_ADDRESS = "0x0000000000000000000000000000000000000000";
        private readonly ILogger<OwnerAnalyzer> _logger;
        public OwnerAnalyzer(ILogger<OwnerAnalyzer> logger)
        {
            _logger = logger;
        }

        public async Task<AnalysisResult> Analyze(TokenAnalysisInfo info)
        {
            try
            {
                var ownerFn = info.TokenContract.GetFunction("owner");
                var owner = await ownerFn.CallAsync<string>();

                if (owner == null || owner == NO_ADDRESS)
                    return AnalysisResult.SafeResult();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Не удалось получить owner() для токена [{info.MainToken()}]");
            }

            return AnalysisResult.UnSafeResult("Не удалось получить owner() для токена");
        }
    }
}
