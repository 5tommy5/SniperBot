using Core.Interfaces;
using CryptoGhegemon.Web.Services;
using Memecoin.Analyzers;

namespace CryptoGhegemon.Web.Hosted;

public class EvmMonitorService : BackgroundService
{
    private readonly ILogger<EvmMonitorService> _logger;
    private readonly ITokenPairMonitor _monitor;
    private readonly AnalyzersFactory _analyzers;
    private readonly MemecoinCache _cache;
    private readonly string _chain;

    public EvmMonitorService(
        ILogger<EvmMonitorService> logger,
        ITokenPairMonitor monitor,
        AnalyzersFactory analyzers,
        MemecoinCache cache)
    {
        _logger = logger;
        _monitor = monitor;
        _analyzers = analyzers;
        _cache = cache;

        _chain = "bsc";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"[{_chain.ToUpper()}] Старт мониторинга...");

        await foreach (var pairInfo in _monitor.MonitorAsync(_chain, stoppingToken))
        {
            _cache.Add(pairInfo);
        }
    }
}