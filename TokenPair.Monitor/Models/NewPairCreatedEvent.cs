﻿using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace TokenPair.Monitor.Models
{
    [Event("PairCreated")]
    internal class NewPairCreatedEvent : IEventDTO
    {
        [Parameter("address", "token0", 1, true)]
        public string Token0 { get; set; }

        [Parameter("address", "token1", 2, true)]
        public string Token1 { get; set; }

        [Parameter("address", "pair", 3, false)]
        public string Pair { get; set; }

        [Parameter("uint256", "", 4, false)]
        public BigInteger Dummy { get; set; }
    }
}
