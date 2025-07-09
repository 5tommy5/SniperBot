namespace Core.Evm.Helpers;

public interface IAbiProvider
{
    string GetFactory(string chain);
    string GetRouter(string chain);
    string GetErc20(string chain);
    string GetPair(string chain);
}