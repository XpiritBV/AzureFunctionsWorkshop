using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RetroGamingFunctionApp.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

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
        Mock<CloudTable> tableMock;

        [TestInitialize]
        public void TestInitialize()
        {
            score = new GameScore() { Game = "Pacman", Nickname = "LX", Points = 1337 };
            receivedEvent = new GameScoreReceivedEvent() { Id = Guid.NewGuid(), Score = score };
            log = new Mock<ILogger>().Object;
            collectorMock = new Mock<IAsyncCollector<SignalRMessage>>();
            messages = collectorMock.Object;
            Uri uri = new UriBuilder(Uri.UriSchemeHttp, "accountname.localhost", 80).Uri;
            tableMock = new Mock<CloudTable>(uri, null);
        }

        [TestMethod]
        public async Task NewHighScoreShouldStoreResultAndSendSignalRMessage()
        {
            // Arrange
            tableMock.Setup(t => t.ExecuteAsync(It.IsAny<TableOperation>())).ReturnsAsync(new TableResult() { Result = null });

            // Act
            await CalculateHighScoreFunction.Run(receivedEvent, tableMock.Object, messages, log);
            
            // Assert
            tableMock.Verify(m => m.ExecuteAsync(It.IsAny<TableOperation>()), Times.Exactly(2));
            collectorMock.Verify(m => m.AddAsync(It.IsAny<SignalRMessage>(), CancellationToken.None), Times.Once);
        }

        [TestMethod]
        public async Task NoHighScoreShouldNotStoreResultNorSendSignalRMessage()
        {
            // Arrange
            tableMock.Setup(t => t.ExecuteAsync(It.IsAny<TableOperation>())).ReturnsAsync(
                new TableResult() {
                    Result = new HighScoreEntry { PartitionKey = score.Game, RowKey = score.Nickname, Points = score.Points + 1 }
                });

            // Act
            await CalculateHighScoreFunction.Run(receivedEvent, tableMock.Object, messages, log);

            // Assert
            tableMock.Verify(m => m.ExecuteAsync(It.IsAny<TableOperation>()), Times.Exactly(1));
            collectorMock.Verify(m => m.AddAsync(It.IsAny<SignalRMessage>(), CancellationToken.None), Times.Never);
        }
    }
}
