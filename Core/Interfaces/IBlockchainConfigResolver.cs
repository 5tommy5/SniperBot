namespace Core.Interfaces;

public interface IBlockchainConfigResolver
{
    BlockchainConfiguration Get(string chain);
}