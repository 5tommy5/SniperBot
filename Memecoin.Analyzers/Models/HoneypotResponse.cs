namespace Memecoin.Analyzers.Models
{
    public class HoneypotResponse
    {
        public HoneypotTokenInfo Token { get; set; }
        public HoneypotTokenInfo WithToken { get; set; }
        public HoneypotSummaryInfo Summary { get; set; }
        public HoneypotResult HoneypotResult { get; set; }
        public bool SimulationSuccess { get; set; }
        public HoneypotSimulationResult SimulationResult { get; set; }
    }

    public class HoneypotTokenInfo
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public int Decimals { get; set; }
        public string Address { get; set; }
        public int TotalHolders { get; set; }
    }

    public class HoneypotSummaryInfo
    {
        public string Risk { get; set; }
        public int RiskLevel { get; set; }
    }

    public class HoneypotResult
    {
        public bool IsHoneypot { get; set; }
    }

    public class HoneypotSimulationResult
    {
        public int BuyTax { get; set; }
        public int SellTax { get; set; }
        public int TransferTax { get; set; }
        public string BuyGas { get; set; }
        public string SellGas { get; set; }
    }
}
