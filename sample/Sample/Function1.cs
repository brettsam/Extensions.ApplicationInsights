using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Extensions.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Sample
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [ApplicationInsightsContext] IAsyncCollector<KeyValuePair<string, string>> appInsightsContext,
            ILogger log)
        {
            // TODO: Can we use a converter to prevent this?
            ApplicationInsightsContext context = (ApplicationInsightsContext)appInsightsContext;

            // TODO: How would this work in other languages?
            await context.AddAsync(new KeyValuePair<string, string>("MyContext", "ABC123"));

            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
