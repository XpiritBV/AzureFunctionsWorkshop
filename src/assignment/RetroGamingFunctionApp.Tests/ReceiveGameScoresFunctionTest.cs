using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RetroGamingFunctionApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace RetroGamingFunctionApp.Tests
{
    [TestClass]
    public class ReceiveGameScoresFunctionTest
    {
        public readonly string playerName = "LX360";

        [TestMethod]
        public async Task RequestWithMultipleScoresShouldSendMultipleMessages()
        {
            // Arrange
            ILogger log = new Mock<ILogger>().Object;
            var mockCollector = new Mock<ICollector<GameScoreReceivedEvent>>();
            ICollector<GameScoreReceivedEvent> collector = mockCollector.Object;
            var request = new Mock<HttpRequest>();

            // Strictly not necessary
            request.Setup(req => req.Method).Returns("POST");
            request.Setup(req => req.Path).Returns("/api/ReceiveGameScoresFunction");

            // Setup content of body for 2 game scores
            string body = "[{'Nickname' : 'King', 'Points' : 42, 'Game' : 'Pacman' }, {'Nickname' : 'Kong', 'Points' : 1337, 'Game' : 'Pacman' }]";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(body);
            writer.Flush();
            stream.Position = 0;
            request.Setup(req => req.Body).Returns(stream);

            // Act
            var response = await ReceiveGameScoresFunction.Run(request.Object, collector, log);
            var resultObject = response as OkObjectResult;

            // Assert
            Assert.IsNotNull(resultObject, "Result object should be of type OkObjectResult");
            Assert.AreEqual<int>((int)HttpStatusCode.OK, resultObject.StatusCode.Value);
            Assert.AreEqual("Processed 2 game scores", resultObject.Value);
            mockCollector.Verify(m => m.Add(It.IsAny<GameScoreReceivedEvent>()), Times.Exactly(2));
        }
    }
}
