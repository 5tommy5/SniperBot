using Core;
using Core.Interfaces;
using Memecoin.Analyzers.Models;

namespace Memecoin.Analyzers.Implementations
{
    public class UnsafeFunctionsAnalyzer : ITokenAnalyzer
    {
        private readonly ITokenMetadataProvider _meta;
        
        public UnsafeFunctionsAnalyzer(ITokenMetadataProvider meta)
        {
            _meta = meta;
        }
        public async Task<AnalysisResult> Analyze(TokenInfo info)
        {
            var functions = await _meta.GetFunctions(info.Chain, info.Address);
            if (functions.Any(f => f.Contains("mint") || f.Contains("blacklist") || f.Contains("setbalance")))
                return AnalysisResult.UnSafeResult("Вредоносные функции");

            return AnalysisResult.SafeResult();
        }
    }
}
