using Microsoft.Extensions.Logging;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using SniperBot.Analyzers;
using SniperBot.Analyzers.Implementations;
using SniperBot.Analyzers.Models;
using SniperBot.Core.Config;
using SniperBot.Core.Extensions;
using SniperBot.Core.Helpers;
using SniperBot.Monitor.Services;
Console.OutputEncoding = System.Text.Encoding.UTF8;

#region configuration
var configManager = new ConfigManager();
var config = configManager.Load();

var loggerFactory = new LoggerFactory();

var monitor = new BlockchainMonitor();

using var client = new HttpClient();

var analyzers = new List<ITokenAnalyzer>()
{
    new HoneypotAnalyzer(client),
    new OwnerAnalyzer(loggerFactory.CreateLogger<OwnerAnalyzer>()),
    new ReservsAnalyzer(),
    new UnsafeFunctionsAnalyzer()
};
#endregion


#region web3
var account = new Account(config.PrivateKey);
var web3 = new Web3(account, config.RpcUrl);

var factory = web3.Eth.GetContract(AbiHelper.GetFactory(), Constants.FACTORY_ADDRESS);
var router = web3.Eth.GetContract(AbiHelper.GetRouter(), Constants.ROUTER_ADDRESS);
#endregion

Console.WriteLine("[*] Мониторим новые пары...");

await foreach (var token in monitor.Monitor(web3, factory))
{
    var mainToken = token.Token1.ToLower() != Constants.WBNB.ToLower() ? token.Token1 : token.Token0;

    var tokenInfo = new TokenAnalysisInfo()
    {
        Token0 = token.Token0,
        Token1 = token.Token1,
        Pair = token.Pair,
        TokenContract = web3.Eth.GetContract(AbiHelper.GetErc20(), mainToken),
        PairContract = web3.Eth.GetContract(AbiHelper.GetPair(), token.Pair)
    };

    var analyzerTasks = new List<Task<AnalysisResult>>();

    foreach (var analyzer in analyzers)
    {
        analyzerTasks.Add(analyzer.Analyze(tokenInfo));
    }

    AnalysisResult result = null;

    while (analyzerTasks.Count > 0)
    {
        var completed = await Task.WhenAny(analyzerTasks);
        analyzerTasks.Remove(completed);

        var analysisResult = await completed;
        if (!analysisResult.IsSafe)
        {
            result = analysisResult;
        }
    }

    if (result is not null && !result.IsSafe)
        Console.WriteLine($"[!] Не нужно было покупать токен [{mainToken}] по причине: {result.Reason}");
    else
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[+] Нужно было бы купить токен [{mainToken}]");
        Console.ResetColor();
    }
}