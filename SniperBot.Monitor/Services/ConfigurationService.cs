using Microsoft.Extensions.Configuration;
using SniperBot.Monitor.Models;

namespace SniperBot.Monitor.Services
{
    internal static class ConfigurationService
    {
        internal static Configuration LoadEnv()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("ConfigFiles/appsettings.json", optional: false)
                .Build();

            var result = new Configuration()
            {
                RpcUrl = config["RPC_URL"],
                PrivateKey = config["PRIVATE_KEY"],
                PublicAddress = config["ADDRESS"]
            };

            return result;
        }
    }
}
