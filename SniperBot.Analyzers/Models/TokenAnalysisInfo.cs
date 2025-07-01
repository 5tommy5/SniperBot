using Nethereum.Contracts;
using SniperBot.Core.Extensions;

namespace SniperBot.Analyzers.Models
{
    public record TokenAnalysisInfo
    {
        public required string Token0 { get; set; }
        public required string Token1 { get; set; }
        public required string Pair { get; set; }

        public required Contract TokenContract { get; set; }
        public required Contract PairContract { get; set; }

        public string MainToken() => Token0 == Constants.WBNB ? Token1 : Token0;
    }
}
