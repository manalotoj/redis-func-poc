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
            //string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            //string responseMessage = string.IsNullOrEmpty(name)
            //    ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //    : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult("done");
        }
    }
}
