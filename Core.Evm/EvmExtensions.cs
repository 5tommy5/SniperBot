using Core.Evm.Helpers;
using Core.Evm.Models;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Evm;

public static class EvmExtensions
{
    public static IServiceCollection AddEvm(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("MonitorConfig");
        services.Configure<EvmBlockchainConfiguration>(section);
        
        services.AddTransient<ITokenMetadataProvider, EvmTokenMetadataProvider>();

        services.AddSingleton<IAbiProvider, AbiProvider>();
        services.AddSingleton<IWeb3Factory, Web3Factory>();
        return services;
    }
}