using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RetroGamingFunctionApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace RetroGamingFunctionApp
{
    [StorageAccount("RetroGamingStorageConnectionAppSetting")]
    public static class ReceiveGameScoresFunction
    {
        [FunctionName("ReceiveGameScoresFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, nameof(HttpMethods.Post), Route = null)] HttpRequest req,
            [Queue("gamescorequeue"), StorageAccount("AzureWebJobsStorage")] ICollector<GameScoreReceivedEvent> message,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            IEnumerable<GameScore> scores = JsonConvert.DeserializeObject<IEnumerable<GameScore>>(requestBody);

            foreach (GameScore score in scores)
            {
                message.Add(new GameScoreReceivedEvent() { Id = Guid.NewGuid(), Score = score });
                log.LogInformation($"Successfully received score of {score.Points} by {score.Nickname} for game {score.Game}.");
            }

            return new OkObjectResult($"Processed {scores.Count()} game scores");
        }
    }
}
