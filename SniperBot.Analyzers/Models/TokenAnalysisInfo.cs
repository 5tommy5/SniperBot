using Nethereum.Contracts;

namespace SniperBot.Analyzers.Models
{
    public record TokenAnalysisInfo
    {
        internal const string WBNB = "0xbb4CdB9CBd36B01bD1cBaEBF2De08d9173bc095c";

        public required string Token0 { get; set; }
        public required string Token1 { get; set; }
        public required string Pair { get; set; }

        public required Contract TokenContract { get; set; }
        public required Contract PairContract { get; set; }

        public string MainToken() => Token0 == WBNB ? Token1 : Token0;
    }
}
