using Azure.Data.Tables;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using AzureFunctions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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
            [Table(TableConfig.Table)] TableClient cloudTable)
        {
            string region = request.Query["region"];
          
            var playerEntities  = cloudTable.QueryAsync<PlayerEntity>(a=>a.PartitionKey == region);
          
            return new OkObjectResult(playerEntities.AsPages());
        }
    }
}
