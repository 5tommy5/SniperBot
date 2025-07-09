using Core;
using Core.Interfaces;
using Memecoin.Analyzers.Models;

namespace Memecoin.Analyzers.Implementations
{
    public class ReservsAnalyzer : ITokenAnalyzer
    {
        private readonly ITokenMetadataProvider _meta;
        private readonly IChainAnalyzerLimitation _limitations;
        
        public ReservsAnalyzer(ITokenMetadataProvider meta, IChainAnalyzerLimitation limitation)
        {
            _meta = meta;
            _limitations = limitation;
        }

        public async Task<AnalysisResult> Analyze(TokenInfo info)
        {
            (var liquidity0, var blockLast) = await _meta.GetReservsAsync(info.Chain, info.Address);
            var minLiquidity = _limitations.GetMinLiquidity(info.Chain);
            var minAge = _limitations.GetMinReservsBlock(info.Chain);
            
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var age = now - blockLast;

            if (liquidity0 < minLiquidity || age < minAge)
                return AnalysisResult.UnSafeResult("Недостаточная ликвидность и/или слишком малое время пары");

            return AnalysisResult.SafeResult();
        }
    }
}
