using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using DarkContoso.DurableFunction.Authentication;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using DarkContoso.DurableFunction.Helpers;

namespace DarkContoso.DurableFunction
{
    public static class DurableFunctionsOrchestration
    {
        [FunctionName("DurableFunctionsOrchestration")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            if (!context.IsReplaying)
            {
                // MS Graph Test Area - Start
                var graphClient = GraphServiceAuth.GetAuthenticatedGraphClient();
                var options = new List<QueryOption>
                {
                    new QueryOption("$top", "1")
                };
                
                var graphResult = graphClient.Users.Request(options).GetAsync().Result;
                log.LogInformation("Graph SDK Result");
                log.LogInformation(graphResult[0].DisplayName);
                // MS Graph Test Area - End
            }
            
            
            var outputs = new List<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>("DurableFunctionsOrchestration_Hello", "Tokyo"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }
    }
}