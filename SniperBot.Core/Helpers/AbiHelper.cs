namespace SniperBot.Core.Helpers
{
    public static class AbiHelper
    {
        private static string? _factoryAbi;
        private static string? _routerAbi;
        private static string? _erc20Abi;
        private static string? _pairAbi;

        public static string GetFactory()
        {
            if (_factoryAbi is not null)
                return _factoryAbi;

            _factoryAbi = File.ReadAllText("ConfigFiles/Abi/Factory.json");

            return _factoryAbi;
        }

        public static string GetRouter()
        {
            if (_routerAbi is not null)
                return _routerAbi;

            _routerAbi = File.ReadAllText("ConfigFiles/Abi/Router.json");

            return _routerAbi;
        }

        public static string GetErc20()
        {
            if (_erc20Abi is not null)
                return _erc20Abi;

            _erc20Abi = File.ReadAllText("ConfigFiles/Abi/ERC20.json");

            return _erc20Abi;
        }

        public static string GetPair()
        {
            if (_pairAbi is not null)
                return _pairAbi;

            _pairAbi = File.ReadAllText("ConfigFiles/Abi/Pair.json");

            return _pairAbi;
        }
    }
}
