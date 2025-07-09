using Core.Implementation;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class CoreExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<IDexAddressProvider, DexAddressProvider>();
        services.AddSingleton<IChainAnalyzerLimitation, ChainAnalyzerLimitationsProvider>();

        return services;
    } 
}