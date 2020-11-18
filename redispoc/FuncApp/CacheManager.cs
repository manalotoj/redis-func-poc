using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace FuncApp
{
    public class CacheManager
    {
        private readonly ConnectionMultiplexer connection;
        private readonly ILogger<CacheManager> logger;

        public CacheManager(ConnectionMultiplexer connection,
            ILogger<CacheManager> logger)
        {
            this.connection = connection;
            this.logger = logger;
        }

        [FunctionName("ClearCache")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var cache = connection.GetDatabase();
            await cache.ExecuteAsync("FLUSHDB");

            return new OkObjectResult("done");
        }
    }
}
