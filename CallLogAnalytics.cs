using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Identity;
using Azure.Monitor.Query;
using Azure.Monitor.Query.Models;

namespace Company.Function
{
    public static class CallLogAnalytics
    {
        [FunctionName("CallLogAnalytics")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var tenantId =  Environment.GetEnvironmentVariable("TENANT_ID"); 
            var clientId =  Environment.GetEnvironmentVariable("API_ID");
            var clientSecret =  Environment.GetEnvironmentVariable("API_KEY"); 
            var workspaceId =  Environment.GetEnvironmentVariable("WORKSPACE_ID");
            var query = Environment.GetEnvironmentVariable("QUERY"); 
            var days = Environment.GetEnvironmentVariable("DAYS"); 

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var client = new LogsQueryClient(credential);

            try
            {

                var result = await client.QueryWorkspaceAsync(workspaceId,query,new QueryTimeRange(TimeSpan.FromDays(days)));

                log.LogInformation("Call LogAnalytics: " + result.Value.Table.Rows.Count + " rows returned");

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
