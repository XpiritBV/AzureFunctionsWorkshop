using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using RetroGamingFunctionApp.Models;

namespace RetroGamingFunctionApp
{
    public static class CalculateHighScoreFunction
    {
        [FunctionName("CalculateHighScoreFunction")]
        public static async Task Run([QueueTrigger("gamescorequeue")]GameScoreReceivedEvent message,
            [Table("HighScores")] CloudTable table,
            [SignalR(HubName = "leaderboardhub")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {message.Id}");

            TableOperation retrieve = TableOperation.Retrieve<HighScoreEntry>(message.Score.Game.ToLower(), message.Score.Nickname);
            TableResult result = await table.ExecuteAsync(retrieve);

            HighScoreEntry entry = (HighScoreEntry)result.Result ?? new HighScoreEntry() 
                { 
                    PartitionKey = message.Score.Game.ToLower(),
                    RowKey = message.Score.Nickname 
                };

            if (entry.Points < message.Score.Points)
            {
                log.LogInformation(entry.Points.ToString());
                entry.ETag = "*";
                entry.Points = message.Score.Points;

                TableOperation store = TableOperation.InsertOrReplace(entry);
                await table.ExecuteAsync(store);

                await signalRMessages.AddAsync(new SignalRMessage()
                {
                    Target = "leaderboardUpdated",
                    Arguments = new string[] { }, 
                });
                await signalRMessages.FlushAsync();
            }
        }
    }
}
