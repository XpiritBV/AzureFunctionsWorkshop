using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RetroGamingFunctionApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RetroGamingFunctionApp.Tests
{
    [TestClass]
    public class RetrieveHighScoreListFunctionTest
    {
        public const string playerName = "LX360";
        public const string gameName = "Pacman";
        public const int top = 1;

        [TestMethod]
        public async Task RequestWithQueryStringShouldReturnTopItems()
        {
            // Arrange
            GameScore score = new GameScore() { Game = "Pacman", Nickname = "LX", Points = 1337 };
            ILogger log = new Mock<ILogger>().Object;
            var request = new Mock<HttpRequest>();
            Uri uri = new UriBuilder(Uri.UriSchemeHttp, "accountname.localhost", 80).Uri;
            var tableMock = new Mock<CloudTable>(uri, null);
            tableMock.Setup(t => t.ExecuteQuery<HighScoreEntry>(It.IsAny<TableQuery<HighScoreEntry>>(), It.IsAny<TableRequestOptions>(), It.IsAny<OperationContext>())).Returns(
                new List<HighScoreEntry> {
                    new HighScoreEntry { PartitionKey = score.Game, RowKey = score.Nickname + "1", Points = score.Points + 1 },
                    new HighScoreEntry { PartitionKey = score.Game, RowKey = score.Nickname + "2", Points = score.Points + 2 },
                    new HighScoreEntry { PartitionKey = score.Game, RowKey = score.Nickname + "3", Points = score.Points + 3 }
                });

            // HTTP request setup
            request.Setup(req => req.Method).Returns("GET");
            request.Setup(req => req.Path).Returns("/api/highscore/" + gameName);
            var parameters = new Dictionary<string, StringValues>() { ["top"] = new StringValues("1") };
            request.Setup(req => req.Query).Returns(new QueryCollection(parameters));

            // Act
            var response = await RetrieveHighScoreListFunction.Run(request.Object, gameName, tableMock.Object, log);
            var resultObject = response as OkObjectResult;

            // Assert
            Assert.IsNotNull(resultObject, "Result object should be of type OkObjectResult");
            Assert.AreEqual<int>((int)HttpStatusCode.OK, resultObject.StatusCode.Value, "Status code should be 200 OK");
            Assert.IsNotNull(resultObject.Value, "Object should be part of response body");
            Assert.AreEqual(top, ((IEnumerable<object>)resultObject.Value).Count(), "Result should have exactly requested number of objects");
        }
    }
}
