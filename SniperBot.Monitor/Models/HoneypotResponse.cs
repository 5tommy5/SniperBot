namespace SniperBot.Monitor.Models
{
    public class HoneypotResponse
    {
        public TokenInfo Token { get; set; }
        public TokenInfo WithToken { get; set; }
        public SummaryInfo Summary { get; set; }
        public HoneypotResult HoneypotResult { get; set; }
        public bool SimulationSuccess { get; set; }
        public SimulationResult SimulationResult { get; set; }
    }

    public class TokenInfo
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public int Decimals { get; set; }
        public string Address { get; set; }
        public int TotalHolders { get; set; }
    }

    public class SummaryInfo
    {
        public string Risk { get; set; }
        public int RiskLevel { get; set; }
    }

    public class HoneypotResult
    {
        public bool IsHoneypot { get; set; }
    }

    public class SimulationResult
    {
        public int BuyTax { get; set; }
        public int SellTax { get; set; }
        public int TransferTax { get; set; }
        public string BuyGas { get; set; }
        public string SellGas { get; set; }
    }

}
