using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;
using SniperBot.Core.Extensions;
using SniperBot.Core.Helpers;
using SniperBot.Monitor.Models;


namespace SniperBot.Monitor.Services
{
    internal class BlockchainMonitor
    {
        internal async IAsyncEnumerable<SnipedPairInfo> Monitor(Web3 web3, Contract factory)
        {
            var eventSignature = Sha3Keccack.Current.CalculateHash("PairCreated(address,address,address,uint256)");
            var filterInput = new NewFilterInput
            {
                Address = new[] { Constants.FACTORY_ADDRESS },
                Topics = new[] { new object[] { "0x" + eventSignature } }
            };

            var filterId = await web3.Eth.Filters.NewFilter.SendRequestAsync(filterInput);

            while (true)
            {
                var changes = await web3.Eth.Filters.GetFilterChangesForEthNewFilter.SendRequestAsync(filterId);

                foreach (var log in changes)
                {
                    var eventAbi = factory.ContractBuilder.ContractABI.Events.FirstOrDefault(e => e.Name == "PairCreated");
                    if (eventAbi == null) continue;

                    var decoded = Event<NewPairCreatedEvent>.DecodeEvent(log);
                    var token0 = decoded.Event.Token0;
                    var token1 = decoded.Event.Token1;
                    var pair = decoded.Event.Pair;

                    var block = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(log.BlockNumber);
                    var timestamp = (long)block.Timestamp.Value;

                    Console.WriteLine($"[🚨] Новая пара: {token0} + {token1} → {pair}");

                    // Пропускаем пары, где ни один токен не WBNB
                    if (token0.ToLower() != Constants.WBNB.ToLower() && token1.ToLower() != Constants.WBNB.ToLower())
                    {
                        Console.WriteLine("⛔ Пара без WBNB — пропускаем.");
                        continue;
                    }

                    var info = new SnipedPairInfo
                    {
                        Token0 = token0,
                        Token1 = token1,
                        Pair = pair,
                        Timestamp = timestamp,
                        Datetime = DateTimeOffset.FromUnixTimeSeconds(timestamp).ToString("yyyy-MM-dd HH:mm:ss")
                    };
                    SnipedLogs.SaveSnipedInfo(info);

                    yield return info;
                }

                await Task.Delay(3000);
            }
        }
    }
}
