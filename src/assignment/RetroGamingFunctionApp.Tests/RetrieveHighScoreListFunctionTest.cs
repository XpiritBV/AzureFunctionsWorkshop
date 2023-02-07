using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RetroGamingFunctionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;

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
            var tableMock = new Mock<TableClient>();

            var page1 = Page<HighScoreEntry>.FromValues(new[] {      
                new HighScoreEntry { PartitionKey = score.Game, RowKey = score.Nickname + "1", Points = score.Points + 1 },
                new HighScoreEntry { PartitionKey = score.Game, RowKey = score.Nickname + "2", Points = score.Points + 2 },
                new HighScoreEntry { PartitionKey = score.Game, RowKey = score.Nickname + "3", Points = score.Points + 3 }
            }, "continuationToken", Mock.Of<Response>());
            
            tableMock.Setup(t => t.Query<HighScoreEntry>(It.IsAny<Expression<Func<HighScoreEntry, bool>>>(),
                    It.IsAny<int?>(), default, default))
                .Returns(Pageable<HighScoreEntry>.FromPages(new[] { page1 }));

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
