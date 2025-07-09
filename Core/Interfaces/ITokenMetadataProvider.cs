namespace Core.Interfaces;

public interface ITokenMetadataProvider
{
    Task<string> GetSymbolAsync(string chain, string tokenAddress);
    Task<string> GetNameAsync(string chain, string tokenAddress);
    Task<string> GetOwnerAsync(string chain, string tokenAddress);
    Task<(decimal reserves, uint blockTimestampLast)> GetReservsAsync(string chain, string tokenAddress);
    Task<List<string>> GetFunctions(string chain, string tokenAddress);
}