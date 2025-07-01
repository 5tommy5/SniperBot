using Nethereum.Web3;
using SniperBot.Analyzers.Models;

namespace SniperBot.Analyzers.Implementations
{
    internal class ReservsAnalyzer : ITokenAnalyzer
    {
        private const int MIN_BNB_LIQUIDITY = 8;
        public async Task<AnalysisResult> Analyze(TokenAnalysisInfo info)
        {
            var reservesFn = info.PairContract.GetFunction("getReserves");
            var reserves = await reservesFn.CallDeserializingToObjectAsync<GetReservesOutput>();

            decimal bnbLiquidity = Web3.Convert.FromWei(reserves.Reserve1);

            if (bnbLiquidity < MIN_BNB_LIQUIDITY)
                return AnalysisResult.UnSafeResult($"Мало ликвидности (< 6 BNB)");

            return AnalysisResult.SafeResult();
        }
    }
}
