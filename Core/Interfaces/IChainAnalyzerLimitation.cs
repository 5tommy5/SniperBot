namespace Core.Interfaces;

public interface IChainAnalyzerLimitation
{
    decimal GetMinLiquidity(string chain);
    uint GetMinReservsBlock(string chain);

}