using System;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
            string game, [Table("HighScores")] TableClient table, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (!Int32.TryParse(req.Query["top"], out int top) || top > MAXENTRIES)
            { 
                top = DEFAULTENTRIES;
            }

            Pageable<HighScoreEntry> queryResults = table.Query<HighScoreEntry>(x=>x.PartitionKey == game, top );

            return new OkObjectResult(queryResults.Take(top).Select(e => new
            {
                Nickname = e.RowKey,
                Points = e.Points
            }));
        }
        
      
    }
}
