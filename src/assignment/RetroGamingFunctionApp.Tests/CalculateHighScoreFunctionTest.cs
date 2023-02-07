using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RetroGamingFunctionApp.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.WindowsAzure.Storage.Table;

namespace RetroGamingFunctionApp.Tests
{
    [TestClass]
    public class CalculateHighScoreFunctionTest
    {
        public readonly string playerName = "LX360";

        GameScore score;
        GameScoreReceivedEvent receivedEvent;
        ILogger log;
        Mock<IAsyncCollector<SignalRMessage>> collectorMock;
        IAsyncCollector<SignalRMessage> messages;
        Mock<TableClient> tableMock;

        [TestInitialize]
        public void TestInitialize()
        {
            score = new GameScore() { Game = "Pacman", Nickname = "LX", Points = 1337 };
            receivedEvent = new GameScoreReceivedEvent() { Id = Guid.NewGuid(), Score = score };
            log = new Mock<ILogger>().Object;
            collectorMock = new Mock<IAsyncCollector<SignalRMessage>>();
            messages = collectorMock.Object;
            tableMock = new Mock<TableClient>();
        }

        [TestMethod]
        public async Task NewHighScoreShouldStoreResultAndSendSignalRMessage()
        {
            // Arrange
            var response = new Mock<Response<HighScoreEntry>>();
            tableMock.Setup(_ => _.GetEntityAsync<HighScoreEntry>(It.IsAny<string>(),It.IsAny<string>(),default, default))
                .ReturnsAsync(response.Object);

            // Act
            await CalculateHighScoreFunction.Run(receivedEvent, tableMock.Object, /*messages,*/ log);
            
            // Assert
            tableMock.Verify(m => m.GetEntityAsync<HighScoreEntry>(It.IsAny<string>(), It.IsAny<string>(), default, default), Times.Exactly(1));
            tableMock.Verify(m => m.UpsertEntityAsync(It.IsAny<HighScoreEntry>(), TableUpdateMode.Merge, default), Times.Exactly(1));
            
            // When signalr is enabled, this test case can be added
            // collectorMock.Verify(m => m.AddAsync(It.IsAny<SignalRMessage>(), CancellationToken.None), Times.Once);
        }

        [TestMethod]
        public async Task NoHighScoreShouldNotStoreResultNorSendSignalRMessage()
        {
            // Arrange
            var response = new Mock<Response<HighScoreEntry>>();
            response.SetupGet(a=>a.Value).Returns(new HighScoreEntry { PartitionKey = score.Game, RowKey = score.Nickname, Points = score.Points + 1 });
            tableMock.Setup(_ => _.GetEntityAsync<HighScoreEntry>(It.IsAny<string>(),It.IsAny<string>(),default, default))
                .ReturnsAsync(response.Object);
            
            // Act
            await CalculateHighScoreFunction.Run(receivedEvent, tableMock.Object, /*messages,*/ log);

            // Assert
            tableMock.Verify(m => m.UpsertEntityAsync(It.IsAny<HighScoreEntry>(),TableUpdateMode.Merge, default), Times.Exactly(0));
            
            // When signalr is enabled, this test case can be added
            //collectorMock.Verify(m => m.AddAsync(It.IsAny<SignalRMessage>(), CancellationToken.None), Times.Never);
        }
    }
}
