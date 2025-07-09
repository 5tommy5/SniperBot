namespace Core;

public class TokenInfo
{
    public string Name { get; set; } = null!;
    public string Symbol { get; set; } = null!;
    public string Address { get; set; } = null!; // основной токен (не WBNB)

    public string Chain { get; set; } = null!;

    public string PairAddress { get; set; } = null!;
    public string Token0 { get; set; } = null!;
    public string Token1 { get; set; } = null!;
    public string QuoteToken { get; set; } = null!; // WBNB, WETH и т.д.
    public long Timestamp { get; set; }
    public DateTime DetectedAt => DateTimeOffset.FromUnixTimeSeconds(Timestamp).UtcDateTime;
}