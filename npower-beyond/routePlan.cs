using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using npower_beyond.Services;
using Microsoft.Extensions.Configuration;

namespace npower_beyond
{
    public static class RoutePlanControllder
    {

        [FunctionName("routePlan")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                        .SetBasePath(context.FunctionAppDirectory)
                        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables()
                         .Build();

            var mapboxKey = config["MapboxKey"];
            if (!int.TryParse(req.Query["seed"], out var seed))
            {
                seed = 0;
            }

            var service = new RoutePlanService(mapboxKey);
            var visits = await service.GetRoutePlan(8, seed);
            return new JsonResult(visits);
        }
    }
}
