using System.Runtime.CompilerServices;
using Core;
using Core.Evm.Helpers;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using TokenPair.Monitor.Models;

namespace TokenPair.Monitor;

public class EvmMonitor : ITokenPairMonitor
{
    private readonly IWeb3Factory _web3Factory;
    private readonly IAbiProvider _abiProvider;
    private readonly ITokenMetadataProvider _metadataProvider;
    private readonly IDexAddressProvider _dexConfigProvider;
    private readonly ILogger<EvmMonitor> _logger;

    public EvmMonitor(
        IWeb3Factory web3Factory,
        IAbiProvider abiProvider,
        ITokenMetadataProvider metadataProvider,
        IDexAddressProvider dexConfigProvider,
        ILogger<EvmMonitor> logger)
    {
        _web3Factory = web3Factory;
        _abiProvider = abiProvider;
        _metadataProvider = metadataProvider;
        _dexConfigProvider = dexConfigProvider;
        _logger = logger;
    }

    public async IAsyncEnumerable<TokenInfo> MonitorAsync(string chain, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var web3 = _web3Factory.Create(chain);
        
        var dexConfig = _dexConfigProvider.Get(chain);
        
        var factoryAddress = dexConfig.FactoryAddress;
        var quoteToken = dexConfig.WrappedNativeToken;

        var factoryAbi = _abiProvider.GetFactory(chain);
        var contract = web3.Eth.GetContract(factoryAbi, factoryAddress);

        var eventSignature = Nethereum.Util.Sha3Keccack.Current.CalculateHash("PairCreated(address,address,address,uint256)");
        var filterInput = new NewFilterInput
        {
            Address = new[] { factoryAddress },
            Topics = new object[][] { new object[] { "0x" + eventSignature } }
        };

        var filterId = await web3.Eth.Filters.NewFilter.SendRequestAsync(filterInput);

        while (!cancellationToken.IsCancellationRequested)
        {
            var logs = await web3.Eth.Filters.GetFilterChangesForEthNewFilter.SendRequestAsync(filterId);

            foreach (var log in logs)
            {
                var eventAbi = contract.ContractBuilder.ContractABI.Events.FirstOrDefault(e => e.Name == "PairCreated");
                if (eventAbi == null) continue;
                
                var decoded = Event<NewPairCreatedEvent>.DecodeEvent(log);
                
                var token0 = decoded.Event.Token0;
                var token1 = decoded.Event.Token1;
                var pair = decoded.Event.Pair;

                var isToken0Quote = string.Equals(token0, quoteToken, StringComparison.OrdinalIgnoreCase);
                var isToken1Quote = string.Equals(token1, quoteToken, StringComparison.OrdinalIgnoreCase);

                if (!isToken0Quote && !isToken1Quote)
                {
                    _logger.LogInformation("No quote token in pair — skipping.");
                    continue;
                }

                var mainToken = isToken0Quote ? token1 : token0;

                var symbol = await _metadataProvider.GetSymbolAsync(chain, mainToken);
                var name = await _metadataProvider.GetNameAsync(chain, mainToken);

                var block = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(log.BlockNumber);
                var timestamp = (long)block.Timestamp.Value;

                var tokenInfo = new TokenInfo
                {
                    Name = name,
                    Symbol = symbol,
                    Address = mainToken,
                    Chain = chain,
                    Token0 = token0,
                    Token1 = token1,
                    QuoteToken = quoteToken,
                    PairAddress = pair,
                    Timestamp = timestamp
                };

                _logger.LogInformation($"New token detected on {chain}: {tokenInfo.Symbol} ({tokenInfo.Name}) → {tokenInfo.PairAddress}");

                yield return tokenInfo;
            }

            await Task.Delay(3000, cancellationToken);
        }
    }
}