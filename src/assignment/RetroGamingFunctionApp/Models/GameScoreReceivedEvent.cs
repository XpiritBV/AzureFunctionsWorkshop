using RetroGamingFunctionApp.Models;
using System;

namespace RetroGamingFunctionApp
{
    public class GameScoreReceivedEvent
    {
        public Guid Id { get; set; }
        public GameScore Score { get; set; }
    }
}