using Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Core.Implementation;

public class DexAddressProvider : IDexAddressProvider
{
    private readonly IConfiguration _config;

    public DexAddressProvider(IConfiguration config)
    {
        _config = config;
    }
    
    public DexAddresses Get(string chain)
    {
        return _config.GetSection($"Dexes:{chain.ToLower()}").Get<DexAddresses>()
               ?? throw new Exception($"DEX config not found for chain: {chain}");
    }
}