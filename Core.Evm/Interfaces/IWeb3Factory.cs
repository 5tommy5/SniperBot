using Nethereum.Web3;

namespace Core.Evm.Helpers;

public interface IWeb3Factory
{
    Web3 Create(string chain);
}