using Azure.Storage.Queues;
using AzureFunctions.Demo.Queue.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.WindowsAzure.Storage.Queue.Protocol;
using Newtonsoft.Json;

namespace AzureFunctions.Demo.Queue.Output
{
    public static class NewPlayerWithCloudQueueMessageOutput
    {
        [FunctionName(nameof(NewPlayerWithCloudQueueMessageOutput))]
        public static IActionResult Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethods.Post),
                Route = null)] Player player,
            [Queue(QueueConfig.NewPlayerItems)] out Player message)
        {
            IActionResult result = null;
            message = null;

            if (string.IsNullOrEmpty(player.Id))
            {
                result = new BadRequestObjectResult("No player data in request.");
            }
            else
            {
                message = player;
                result = new AcceptedResult();
            }

            return result;
        }
    }
}
