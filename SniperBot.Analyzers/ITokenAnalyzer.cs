using SniperBot.Analyzers.Models;

namespace SniperBot.Analyzers
{
    public interface ITokenAnalyzer
    {
        Task<AnalysisResult> Analyze(TokenAnalysisInfo info);
    }
}
