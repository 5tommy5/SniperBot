using Microsoft.Extensions.Configuration;
using SniperBot.Core.Models;

namespace SniperBot.Core.Config
{
    public class ConfigManager
    {
        private Configuration _config;
        public Configuration Load()
        {
            if (_config is not null)
                return _config;

            var config = new ConfigurationBuilder()
                .AddJsonFile("ConfigFiles/appsettings.json", optional: false)
                .Build();

            _config = new Configuration()
            {
                RpcUrl = config["RPC_URL"],
                PrivateKey = config["PRIVATE_KEY"],
                PublicAddress = config["ADDRESS"]
            };

            return _config;
        }
    }
}
