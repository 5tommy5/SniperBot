using Core.Evm.Models;
using Core.Interfaces;
using Nethereum.Web3;

namespace Core.Evm.Helpers;

public class Web3Factory : IWeb3Factory
{
    private readonly IBlockchainConfigResolver _configResolver;
    private readonly Dictionary<string, Web3> _cache = new();

    public Web3Factory(IBlockchainConfigResolver configResolver)
    {
        _configResolver = configResolver;
    }

    public Web3 Create(string chain)
    {
        chain = chain.ToLower();

        if (_cache.TryGetValue(chain, out var cached))
            return cached;

        var config = _configResolver.Get(chain);

        if (config is not EvmBlockchainConfiguration evmConfig)
            throw new NotSupportedException($"Web3 is only supported for EVM chains. Chain: {chain}");

        var web3 = new Web3(new Nethereum.Web3.Accounts.Account(evmConfig.PrivateKey), evmConfig.RpcUrl);

        _cache[chain] = web3;

        return web3;
    }
}