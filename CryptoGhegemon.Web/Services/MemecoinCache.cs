using Core;

namespace CryptoGhegemon.Web.Services;

public class MemecoinCache
{
    private readonly List<TokenInfo> _memecoinCache = new List<TokenInfo>();

    public void Add(TokenInfo tokenFullInfo)
    {
        _memecoinCache.Add(tokenFullInfo);
    }

    public List<TokenInfo> Get(int amount)
    {
        return _memecoinCache.Take(amount).ToList();
    }
}