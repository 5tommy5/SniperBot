using Nethereum.Web3;
using SniperBot.Monitor.Models;

namespace SniperBot.Monitor.Services
{
    public class TokenAnalyzerService
    {
        private readonly Web3 _web3;
        private readonly string _erc20Abi;
        private readonly string _pairAbi;
        private readonly string _wbnb;
        private readonly HoneypotCheckService _honeypotCheckService;

        public TokenAnalyzerService(Web3 web3, string erc20Abi, string pairAbi, string wbnb)
        {
            _web3 = web3;
            _erc20Abi = erc20Abi;
            _pairAbi = pairAbi;
            _wbnb = wbnb.ToLower();
            _honeypotCheckService = new HoneypotCheckService();
        }

        public async Task<TokenAnalysisResult> AnalyzeAsync(string token, string pairAddress)
        {
            var tokenContract = _web3.Eth.GetContract(_erc20Abi, token);
            var pairContract = _web3.Eth.GetContract(_pairAbi, pairAddress);

            try
            {
                // 🔐 Вредоносные функции
                var functions = tokenContract.ContractBuilder.ContractABI.Functions.Select(f => f.Name.ToLower());
                if (functions.Any(f => f.Contains("mint") || f.Contains("blacklist") || f.Contains("setbalance")))
                    return Fail("⚠️ Вредоносные функции");

                // 👑 Проверка владельца
                try
                {
                    var ownerFn = tokenContract.GetFunction("owner");
                    var owner = await ownerFn.CallAsync<string>();

                    if (owner != null && owner != "0x0000000000000000000000000000000000000000")
                        return Fail("❌ Владелец не отказался от токена");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Не удалось получить owner() для токена {token}: {ex.Message}");
                }

                // 💧 Проверка ликвидности
                var reservesFn = pairContract.GetFunction("getReserves");
                var reserves = await reservesFn.CallDeserializingToObjectAsync<GetReservesOutput>();

                decimal bnbLiquidity = token.ToLower() == _wbnb
                    ? Web3.Convert.FromWei(reserves.Reserve0)
                    : Web3.Convert.FromWei(reserves.Reserve1);

                if (bnbLiquidity < 6)
                    return Fail("⚠️ Мало ликвидности (< 6 BNB)");

                try
                {
                    (var honeypotNotRisky, var comment) = await _honeypotCheckService.AnalyzeAsync(token);
                    if (!honeypotNotRisky) return Fail($"[!] Honeypot: {comment}");
                }
                catch
                {
                    return Fail("🐝 Honeypot: исключение при попытке продать");
                }

                return new TokenAnalysisResult { IsSafe = true, Reason = $"✅ OK, ликвидность: {bnbLiquidity:F2} BNB" };
            }
            catch (Exception ex)
            {
                return Fail("❌ Ошибка анализа: " + ex.Message);
            }
        }

        private TokenAnalysisResult Fail(string reason) =>
            new TokenAnalysisResult { IsSafe = false, Reason = reason };
    }
}
