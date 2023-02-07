# Unit testing Azure Functions

## Goal 🎯

The goal of this lesson is to create unit tests for the Azure Functions in our solution. You will learn how to approach writing a unit test for the `Run` method of your function and how to take care of the external dependencies from bindings and triggers.

> 📝 __Tip__ - If you're stuck at any point you can have a look at the [source code](../src/assignment) in this repository.

This lesson consists of the following exercises:

|Nr|Exercise
|-|-
|1|[Setup a unit test project](#1-setup-a-unit-test-project)
|2|[Writing tests for Azure Functions](#2-writing-tests-for-azure-functions)
|3|[Writing additional tests](#3-writing-additional-tests)

## 1. Setup a unit test project

In this exercise, you will add a unit test project to the solution for the Highscore application from the previous lab. 

### Steps

1. Make sure you have built at least the `ReceiveGameScoresFunction` function from the previous lab. 

    > 📝 __Tip__ - If you were not able to complete the previous labe, you can choose to __copy__ the completed solution from the cloned `AzureFunctionsWorkshop` repository on GitHub. Remove the project `RetroGamingFunctionApp.Tests` before continuing.

2. Create a new C# Unit Test project named `RetroGamingFunctionApp.Tests` to the root of the `src/assignment` folder. This lab assumes using the MSTest framework, but you are free to choose the one you are most used to. Add a reference to the `RetroGamingFunctionApp` project, so we can write code to test the function's implementation. Also, add a NuGet package for `Moq` to act as our mock framework for stubbing and mocking types to isolate our function from external dependencies. 

## 2. Writing tests for Azure Functions

This exercise you are going to create a unit test to verify the logic for receiving game scores in an HTTP request. This involves creating mock objects for the external dependencies you would normally have under normal execution, arranging expected behavior of that dependencies and making assertions on the correctness of the outcome.

### Steps

1. Rename the `UnitTest1.cs` file to `ReceiveGameScoresFunctionTest.cs` and the class name in it to `ReceiveGameScoresFunctionTest`. Now we are ready to start writing a unit test. Examine the implementation for `ReceiveGameScoresFunction.Run` and check the parameters of the method. 

    > 🔎 **Observation** - Attributes related to bindings and triggers are irrelevant outside the Azure Functions runtime.

2. Rename the existing unit test to `RequestWithMultipleScoresShouldSendMultipleMessages`. Create three sections to the unit test for arrange, act and assert by adding comments:
    
    ```c#
    // Arrange
    // Act
    // Assert
    ```
3. In order to mock the dependencies in the list of arguments for the `Run` method, you will need to add references to the NuGet packages with abstractions for logging and HTTP: `Microsoft.AspNetCore.Http.Abstractions` and `Microsoft.Extensions.Logging.Abstractions`.
4. Create mock objects for the `ILogger`, `HttpRequest` and `ICollector<GameScoreReceivedEvent>` dependencies in the `// Assert` section. Here are some fragments to help you out:

    ```C#
    ILogger log = new Mock<ILogger>().Object;
    var mockCollector = new Mock<ICollector<GameScoreReceivedEvent>>();
    ICollector<GameScoreReceivedEvent> collector = mockCollector.Object;
    var request = new Mock<HttpRequest>();
    ```
5. Since the `HttpRequest` object will have the `Body` property evaluated, we need to arrange the setup of that property. Use the `Setup` method on the mock object to return a JSON string as a stream.

    ```c#
    string body = "[{'Nickname' : 'King', 'Points' : 42, 'Game' : 'Pacman' }, {'Nickname' : 'Kong', 'Points' : 1337, 'Game' : 'Pacman' }]";
    var stream = new MemoryStream();
    var writer = new StreamWriter(stream);
    writer.Write(body);
    writer.Flush();
    stream.Position = 0;
    request.Setup(req => req.Body).Returns(stream);
    ```
6. Exercise the `Run` method of the `ReceiveGameScoresFunction` by calling it in the `Act` section:

    ```c#
    var response = await ReceiveGameScoresFunction.Run(request.Object, collector, log);
    ```
7. Make assertions on the intended results of this call. We have returned two `GameScore` objects serialized as JSON in the body of the request. According to our implementation logic this should result in two messages of type `GameScoreReceivedEvent` on the message bus. The output binding for the storage queue `gamescorequeue` is of type `ICollector<GameScoreReceivedEvent>` and must therefore have had two calls to the `Add` method. The total list of assertions is
    - Result object returned from the `Run` method should not be null.
    - Result object should represent a 200 OK response.
    - Result object should have a message corresponding to `Processed 2 game scores`.
    - Mock object for storage queue collector should have received exactly two calls.

    > 📝 __Tip__ - You can take a peek at the assertions in the `Assert` section of the completed solution if you are not familiar with MSTest assertions or verifying mocks using Moq. 

8. Run your unit test and fix any errors.

## 3. Writing additional tests

This optional exercise will give you some practice to write unit tests for more complicated scenarios and dependencies. You can create tests for queues and tables, SignalR messages and other forms of HTTP requests. 

### Steps

1. Add a new class `CalculateHighScoreFunctionTest` in the unit test project. Add two methods `NewHighScoreShouldStoreResultAndSendSignalRMessage` and `NoHighScoreShouldNotStoreResultNorSendSignalRMessage` for testing the `Run` method in two cases: 
    - Received game score is a new high score for the player and game
    - Received game score is not high enough for a new high score

2. Declare class fields to hold references to the mock objects and test data we are going to use:
    
    ```c#
    GameScore score;
    GameScoreReceivedEvent receivedEvent;
    ILogger log;
    Mock<IAsyncCollector<SignalRMessage>> collectorMock;
    IAsyncCollector<SignalRMessage> messages;
    Mock<TableClient> tableMock;
    ```

3. Add a `TestInitialize` method to initialize the mock objects and test objects for each individual test execution:

    ```c#
    score = new GameScore() { Game = "Pacman", Nickname = "LX", Points = 1337 };
    receivedEvent = new GameScoreReceivedEvent() { Id = Guid.NewGuid(), Score = score };
    log = new Mock<ILogger>().Object;
    collectorMock = new Mock<IAsyncCollector<SignalRMessage>>();
    messages = collectorMock.Object;
    tableMock = new Mock<TableClient>();
    ```

4. Go to the first unit test method and include the setup of the `TableClient` mock to return an empty result for the query:

    ```c#
    // Arrange
    var response = new Mock<Response<HighScoreEntry>>();
    tableMock.Setup(_ => _.GetEntityAsync<HighScoreEntry>(It.IsAny<string>(),It.IsAny<string>(),default, default))
                .ReturnsAsync(response.Object);
    ```
5. Exercise the `Run` method
6. Write assertions for the table and collector mock objects:
    - The `TableClient` mock should have been called twice: to read the entity and to insert or update a `HighScoreEntry` object.
    - The `IAsyncCollector<SignalRMessage>` mock should be called once.

7. Implement the second unit test for this class as well to test the case when there is no new high score. The table mock should be arranged to return a `HighScoreEntry` object on a call to `GetEntityAsync`:

    ```c#
    // Arrange
    var response = new Mock<Response<HighScoreEntry>>();
    response.SetupGet(a=>a.Value).Returns(new HighScoreEntry { PartitionKey = score.Game, RowKey = score.Nickname, Points = score.Points + 1 });
    tableMock.Setup(_ => _.GetEntityAsync<HighScoreEntry>(It.IsAny<string>(),It.IsAny<string>(),default, default))
        .ReturnsAsync(response.Object);
            
    ```

8. Make the following assertions:
    - The table mock is called only once (for the initial `ExecuteAsync` call)
    - The collector never has a call to `Add` as there was no new high score and no need to send a message to the SignalR hub.

9. Run all unit tests again to check whether they succeed. Fix any errors as usual.

10. If you feel brave enough, try and create a unit test class `RetrieveHighScoreListFunctionTest` with a method `RequestWithQueryStringShouldReturnTopItems` to test the logic for the `RetrieveHighScoreListFunction.Run` method. Here are some hints:
    - It is important to setup the `HttpRequest` object for the     `GET` request. The tricky part is to include a dictionary for the query string values:
        ```c#
        request.Setup(req => req.Method).Returns("GET");
        request.Setup(req => req.Path).Returns("/api/highscore/" + gameName);
        var parameters = new Dictionary<string, StringValues>() { ["top"] = new StringValues("1") };
        request.Setup(req => req.Query).Returns(new QueryCollection(parameters));
        ```
    - The assertions are the same as for the first unit test you created in this lab. Additionally, add an assertion for the number of objects in the response:
        ```c#
        Assert.AreEqual(top, ((IEnumerable<object>)resultObject.Value).Count(), "Result should have exactly requested number of objects");
        ```
11. Run all unit tests again, check the code coverage and feel free to add additional test to reach a potential 100%.

---
[◀ back to README](../README.md)