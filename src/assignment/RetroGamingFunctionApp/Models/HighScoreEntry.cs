using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroGamingFunctionApp.Models
{
    public class HighScoreEntry : TableEntity
    {
        public int Points { get; set; }
    }
}
