using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using AzureFunctions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos.Table;

namespace AzureFunctions.Table.Input
{
    public static class GetPlayersByRegionCloudTableInput
    {
        [FunctionName(nameof(GetPlayersByRegionCloudTableInput))]
        public static IActionResult Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethods.Get),
                Route = "players")] HttpRequest request,
            [Table(TableConfig.Table)] CloudTable cloudTable)
        {
            string region = request.Query["region"];
            var regionFilter = new TableQuery<PlayerEntity>()
                .Where(
                    TableQuery.GenerateFilterCondition(
                        "PartitionKey", 
                        QueryComparisons.Equal,
                        region));
            var playerEntities = cloudTable.ExecuteQuery<PlayerEntity>(regionFilter);

            return new OkObjectResult(playerEntities);
        }
    }
}
