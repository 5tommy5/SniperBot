using Nethereum.Contracts;

namespace SniperBot.Monitor.Services
{
    internal static class TokenSniperValidation
    {
        internal static async Task<bool> IsSafeToken(Contract tokenContract)
        {
            try
            {
                var functions = tokenContract.ContractBuilder.ContractABI.Functions;
                foreach (var fn in functions)
                {
                    var name = fn.Name.ToLower();
                    if (name.Contains("mint") || name.Contains("setbalance") || name.Contains("blacklist"))
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
