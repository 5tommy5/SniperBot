using Microsoft.Extensions.DependencyInjection;
using SniperBot.Analyzers.Implementations;

namespace SniperBot.Analyzers
{
    public static class AnalyzersExtensions
    {
        public static IServiceCollection AddTokenAnalyzers(this IServiceCollection services)
        {
            services.AddScoped<ITokenAnalyzer, UnsafeFunctionsAnalyzer>();
            services.AddScoped<ITokenAnalyzer, HoneypotAnalyzer>();
            services.AddScoped<ITokenAnalyzer, ReservsAnalyzer>();
            services.AddScoped<ITokenAnalyzer, OwnerAnalyzer>();
            return services;
        }
    }
}
