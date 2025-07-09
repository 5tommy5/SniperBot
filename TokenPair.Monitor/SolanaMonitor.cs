using Core;
using Core.Interfaces;

namespace TokenPair.Monitor;

public class SolanaMonitor : ITokenPairMonitor
{
    public IAsyncEnumerable<TokenInfo> MonitorAsync(string chain, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}