using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using SniperBot.Analyzers;
using SniperBot.Analyzers.Models;
using SniperBot.Monitor.Extensions;
using SniperBot.Monitor.Services;
Console.OutputEncoding = System.Text.Encoding.UTF8;

var config = ConfigurationService.LoadEnv();

var account = new Account(config.PrivateKey);
var web3 = new Web3(account, config.RpcUrl);


var factory = web3.Eth.GetContract(Abi.GetFactory(), Constants.FACTORY_ADDRESS);
var router = web3.Eth.GetContract(Abi.GetRouter(), Constants.ROUTER_ADDRESS);

var monitor = new BlockchainMonitor();

Console.WriteLine("[*] Мониторим новые пары...");

var analyzers = new List<ITokenAnalyzer>();

await foreach (var token in monitor.Monitor(web3, factory))
{
    var mainToken = token.Token1.ToLower() != Constants.WBNB.ToLower() ? token.Token1 : token.Token0;

    var tokenInfo = new TokenAnalysisInfo()
    {
        Token0 = token.Token0,
        Token1 = token.Token1,
        Pair = token.Pair,
        TokenContract = web3.Eth.GetContract(Abi.GetErc20(), mainToken),
        PairContract = web3.Eth.GetContract(Abi.GetPair(), token.Pair)
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
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[!] Не нужно было покупать токен [{mainToken}] по причине: {result.Reason}");
        Console.ResetColor();
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[+] Нужно было бы купить токен [{mainToken}]");
        Console.ResetColor();
    }
}