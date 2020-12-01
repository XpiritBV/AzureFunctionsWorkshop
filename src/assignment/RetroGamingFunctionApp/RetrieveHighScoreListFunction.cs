using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;
using RetroGamingFunctionApp.Models;

namespace RetroGamingFunctionApp
{
    public static class RetrieveHighScoreListFunction
    {
        private const int MAXENTRIES = 20;
        private const int DEFAULTENTRIES = 10;

        [FunctionName("RetrieveHighScoreList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "highscore/{game}")] HttpRequest req,
            string game, [Table("HighScores")] CloudTable table, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (!Int32.TryParse(req.Query["top"], out int top) || top > MAXENTRIES)
            { 
                top = DEFAULTENTRIES;
            }

            var query = new TableQuery<HighScoreEntry>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, game));
            var result = table.ExecuteQuery<HighScoreEntry>(query);

            return new OkObjectResult(result.OrderByDescending(e => e.Points).Take(top).Select(e => new
            {
                Nickname = e.RowKey,
                Points = e.Points
            }));
        }
    }
}
