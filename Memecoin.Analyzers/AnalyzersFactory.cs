using Core;

namespace Memecoin.Analyzers;

public class AnalyzersFactory
{
    private readonly IEnumerable<ITokenAnalyzer> _analyzers;
    public AnalyzersFactory(IEnumerable<ITokenAnalyzer> analyzers)
    {
        _analyzers = analyzers;
    }

    public IEnumerable<ITokenAnalyzer> GetAnalyzers(ChainEnum chain)
    {
        return _analyzers.Where(analyzer => analyzer.SupportedChains is null || analyzer.SupportedChains.Contains(chain));
    }
}