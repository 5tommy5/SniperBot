using Core;
using Memecoin.Analyzers.Models;

namespace Memecoin.Analyzers
{
    public interface ITokenAnalyzer
    {
        ChainEnum[]? SupportedChains => null;
        Task<AnalysisResult> Analyze(TokenInfo info);
    }
}
