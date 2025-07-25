﻿using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace Core.Models
{
    [FunctionOutput]
    public class GetReservesOutput : IFunctionOutputDTO
    {
        [Parameter("uint112", "_reserve0", 1)]
        public BigInteger Reserve0 { get; set; }

        [Parameter("uint112", "_reserve1", 2)]
        public BigInteger Reserve1 { get; set; }

        [Parameter("uint32", "_blockTimestampLast", 3)]
        public uint BlockTimestampLast { get; set; }
    }
}
