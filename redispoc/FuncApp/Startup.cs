using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.IO;

[assembly: FunctionsStartup(typeof(FuncApp.Startup))]

namespace FuncApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .Build();

            string connString = config["RedisConnectionString"];
            var lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(connString);
            });

            builder.Services.AddSingleton(lazyConnection.Value);

            builder.Services.AddScoped(typeof(CacheManager));
        }
    }
}