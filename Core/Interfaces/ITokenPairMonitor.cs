namespace Core.Interfaces;

public interface ITokenPairMonitor
{
    IAsyncEnumerable<TokenInfo> MonitorAsync(string chain, CancellationToken cancellationToken = default);
}