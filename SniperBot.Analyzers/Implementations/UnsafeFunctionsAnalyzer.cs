using SniperBot.Analyzers.Models;

namespace SniperBot.Analyzers.Implementations
{
    public class UnsafeFunctionsAnalyzer : ITokenAnalyzer
    {
        public async Task<AnalysisResult> Analyze(TokenAnalysisInfo info)
        {
            var functions = info.TokenContract.ContractBuilder.ContractABI.Functions.Select(f => f.Name.ToLower());
            if (functions.Any(f => f.Contains("mint") || f.Contains("blacklist") || f.Contains("setbalance")))
                return AnalysisResult.UnSafeResult("Вредоносные функции");

            return AnalysisResult.SafeResult();
        }
    }
}
