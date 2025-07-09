using Microsoft.Extensions.Configuration;

namespace Core.Evm.Helpers;

public class AbiProvider : IAbiProvider
{
    private readonly string _abiRootPath;
    private readonly Dictionary<string, string> _cache = new();

    public AbiProvider()
    {
        _abiRootPath = Path.Combine(AppContext.BaseDirectory, "Abi");
    }

    public string GetFactory(string chain)
        => GetAbi(chain, "Factory.json");

    public string GetRouter(string chain)
        => GetAbi(chain, "Router.json");

    public string GetErc20(string chain)
        => GetAbi(chain, "ERC20.json");

    public string GetPair(string chain)
        => GetAbi(chain, "Pair.json");

    private string GetAbi(string chain, string fileName)
    {
        var key = $"{chain.ToLower()}:{fileName.ToLower()}";

        if (_cache.TryGetValue(key, out var cached))
            return cached;

        var fullPath = Path.Combine(_abiRootPath, chain, fileName);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"ABI not found: {fullPath}");

        var abi = File.ReadAllText(fullPath);
        _cache[key] = abi;
        return abi;
    }
}