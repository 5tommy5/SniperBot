using Core.Interfaces;
using CryptoGhegemon.Web.Hosted;
using CryptoGhegemon.Web.Services;

namespace CryptoGhegemon.Web;

public static class WebExtensions
{
    public static IServiceCollection AddWeb(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddControllersWithViews();

        return services;
    }

    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddSingleton<IBlockchainConfigResolver, BlockchainConfigResolver>();
        services.AddSingleton<MemecoinCache>();
        
        return services;
    }
    
    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<EvmMonitorService>();
        
        return services;
    }
}