using Core;
using Core.Evm.Models;
using Core.Interfaces;

namespace CryptoGhegemon.Web.Services;

public class BlockchainConfigResolver : IBlockchainConfigResolver
{
    private readonly IConfiguration _configuration;
    private readonly Dictionary<string, BlockchainConfiguration> _cache = new();

    public BlockchainConfigResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public BlockchainConfiguration Get(string chain)
    {
        var key = chain.ToLowerInvariant();

        if (_cache.TryGetValue(key, out var cached))
            return cached;

        var section = _configuration.GetSection($"Chains:{key}");

        if (!section.Exists())
            throw new Exception($"Chain config not found: {chain}");

        var chainType = section.GetValue<string>("Chain")?.ToLowerInvariant();

        BlockchainConfiguration config = chainType switch
        {
            "bsc" or "ethereum" or "polygon" or "arbitrum" => 
                section.Get<EvmBlockchainConfiguration>() 
                ?? throw new Exception($"Failed to bind EVM config for {chain}"),

            _ => throw new NotSupportedException($"Unsupported chain type: {chainType}")
        };

        _cache[key] = config;
        return config;
    }
}