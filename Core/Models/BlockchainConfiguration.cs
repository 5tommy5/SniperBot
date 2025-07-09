namespace Core;

public abstract class BlockchainConfiguration
{
    public string RpcUrl { get; set; } = null!;
    public string PublicAddress { get; set; } = null!;
    public string Chain { get; set; } = null!;
}