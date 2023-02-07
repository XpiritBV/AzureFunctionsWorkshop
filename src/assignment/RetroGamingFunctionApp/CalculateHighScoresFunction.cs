using System;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using RetroGamingFunctionApp.Models;

namespace RetroGamingFunctionApp
{
    public static class CalculateHighScoreFunction
    {
        [FunctionName("CalculateHighScoreFunction")]
        public static async Task Run([QueueTrigger("gamescorequeue")]GameScoreReceivedEvent message,
            [Table("HighScores")] TableClient table,
            /*[SignalR(HubName = "leaderboardhub")] IAsyncCollector<SignalRMessage> signalRMessages,*/
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {message.Id}");
      
            HighScoreEntry entry;
            
            try
            {
                var result = await table.GetEntityAsync<HighScoreEntry>(message.Score.Game.ToLower(), message.Score.Nickname);
                entry = result.Value;
            }
            catch (RequestFailedException e) // item does not exist
            {
                entry = new HighScoreEntry
                { 
                    PartitionKey = message.Score.Game.ToLower(),
                    RowKey = message.Score.Nickname 
                };
            }

            if (entry.Points < message.Score.Points)
            {
                log.LogInformation(entry.Points.ToString());
                entry.ETag = ETag.All;
                entry.Points = message.Score.Points;

                var store = await table.UpsertEntityAsync(entry);
             
                /*
                 await signalRMessages.AddAsync(new SignalRMessage()
                {
                    Target = "leaderboardUpdated",
                    Arguments = new string[] { }, 
                });
                await signalRMessages.FlushAsync();
                */
            }
        }
    }
}
