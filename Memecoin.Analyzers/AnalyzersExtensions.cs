using Memecoin.Analyzers.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Memecoin.Analyzers
{
    public static class AnalyzersExtensions
    {
        public static IServiceCollection AddTokenAnalyzers(this IServiceCollection services)
        {
            services.AddTransient<ITokenAnalyzer, UnsafeFunctionsAnalyzer>();
            services.AddTransient<ITokenAnalyzer, HoneypotAnalyzer>();
            services.AddTransient<ITokenAnalyzer, ReservsAnalyzer>();
            services.AddTransient<ITokenAnalyzer, OwnerAnalyzer>();
            
            services.AddTransient<AnalyzersFactory>();
            return services;
        }
    }
}
