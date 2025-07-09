using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace TokenPair.Monitor;

public static class MonitorExtensions
{
    public static IServiceCollection AddMonitors(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ITokenPairMonitor, EvmMonitor>();
        
        return serviceCollection;
    }
}