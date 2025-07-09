using Core.Interfaces;
using Core.Models;
using Nethereum.Web3;

namespace Core.Evm.Helpers;

public class EvmTokenMetadataProvider : ITokenMetadataProvider
{
    private readonly IWeb3Factory _web3Factory;
    private readonly IAbiProvider _abiProvider;

    public EvmTokenMetadataProvider(IWeb3Factory web3Factory, IAbiProvider abiProvider)
    {
        _web3Factory = web3Factory;
        _abiProvider = abiProvider;
    }

    public Task<List<string>?> GetFunctions(string chain, string tokenAddress)
    {
        try
        {
            var web3 = _web3Factory.Create(chain);
            var abi = _abiProvider.GetErc20(chain);
            var contract = web3.Eth.GetContract(abi, tokenAddress);

            return Task.FromResult(contract.ContractBuilder.ContractABI.Functions?.Select(f => f.Name.ToLower()).ToList());
        }
        catch
        {
            return Task.FromResult(new List<string>());
        }
    }
    
    public async Task<(decimal reserves, uint blockTimestampLast)> GetReservsAsync(string chain, string tokenAddress)
    {
        try
        {
            var web3 = _web3Factory.Create(chain);
            var abi = _abiProvider.GetErc20(chain);
            var contract = web3.Eth.GetContract(abi, tokenAddress);
            var function = contract.GetFunction("getReserves");
            var reserves = await function.CallDeserializingToObjectAsync<GetReservesOutput>();
            return (Web3.Convert.FromWei(reserves.Reserve0), reserves.BlockTimestampLast);
        }
        catch
        {
            return (0, 0);
        }
    }

    public async Task<string> GetSymbolAsync(string chain, string tokenAddress)
    {
        try
        {
            var web3 = _web3Factory.Create(chain);
            var abi = _abiProvider.GetErc20(chain);
            var contract = web3.Eth.GetContract(abi, tokenAddress);
            var function = contract.GetFunction("symbol");
            return await function.CallAsync<string>();
        }
        catch
        {
            return "UNKNOWN";
        }
    }
    
    public async Task<string> GetOwnerAsync(string chain, string tokenAddress)
    {
        try
        {
            var web3 = _web3Factory.Create(chain);
            var abi = _abiProvider.GetErc20(chain);
            var contract = web3.Eth.GetContract(abi, tokenAddress);
            var function = contract.GetFunction("owner");
            return await function.CallAsync<string>();
        }
        catch
        {
            return "UNKNOWN";
        }
    }

    public async Task<string> GetNameAsync(string chain, string tokenAddress)
    {
        try
        {
            var web3 = _web3Factory.Create(chain);
            var abi = _abiProvider.GetErc20(chain);
            var contract = web3.Eth.GetContract(abi, tokenAddress);
            var function = contract.GetFunction("name");
            return await function.CallAsync<string>();
        }
        catch
        {
            return "Unknown Token";
        }
    }
}