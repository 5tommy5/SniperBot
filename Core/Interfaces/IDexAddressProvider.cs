namespace Core.Interfaces;

public interface IDexAddressProvider
{
    public DexAddresses Get(string chain);
}