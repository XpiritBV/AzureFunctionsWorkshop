using System;
using Azure;
using Azure.Data.Tables;

namespace AzureFunctions.Models
{
    public class PlayerEntity : ITableEntity
    {
        public PlayerEntity()
        {}
        public PlayerEntity(
            string region,
            string id,
            string nickName,
            string email) 
            
        {
            PartitionKey = region;
            Region = region;
            Id = id;
            NickName = nickName;
            Email = email; 
        }

        public string Id { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Region { get; set; }

        public void SetKeys()
        {
            PartitionKey = Region;
            RowKey = Id;
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}