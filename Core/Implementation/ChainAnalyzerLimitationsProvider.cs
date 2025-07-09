using Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Core.Implementation;

public class ChainAnalyzerLimitationsProvider : IChainAnalyzerLimitation
{
    private readonly IConfiguration _config;

    public ChainAnalyzerLimitationsProvider(IConfiguration config)
    {
        _config = config;
    }
    
    public decimal GetMinLiquidity(string chain)
    {
        var config = _config.GetSection($"Limitations:{chain.ToLower()}").Get<ChainLimitations>()
               ?? throw new Exception($"Limitations config not found for chain: {chain}");

        return config.Liquidity;
    }

    public uint GetMinReservsBlock(string chain)
    {
        var config = _config.GetSection($"Limitations:{chain.ToLower()}").Get<ChainLimitations>()
                     ?? throw new Exception($"Limitations config not found for chain: {chain}");

        return config.BlockTime;
    }
}