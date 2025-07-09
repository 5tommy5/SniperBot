using Core;
using Core.Interfaces;
using Memecoin.Analyzers.Models;
using Microsoft.Extensions.Logging;


namespace Memecoin.Analyzers.Implementations
{
    public class OwnerAnalyzer : ITokenAnalyzer
    {
        private const string NO_ADDRESS = "0x0000000000000000000000000000000000000000";
        private readonly ILogger<OwnerAnalyzer> _logger;
        private readonly ITokenMetadataProvider _metadata;
        public OwnerAnalyzer(ITokenMetadataProvider metadata, ILogger<OwnerAnalyzer> logger)
        {
            _metadata = metadata;
            _logger = logger;
        }

        public async Task<AnalysisResult> Analyze(TokenInfo info)
        {
            try
            {
                var owner = await _metadata.GetOwnerAsync(info.Chain, info.Address);

                if (owner == null || owner == NO_ADDRESS)
                    return AnalysisResult.SafeResult();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Не удалось получить owner() для токена [{info.Address}]");
            }

            return AnalysisResult.UnSafeResult("Не удалось получить owner() для токена");
        }
    }
}
