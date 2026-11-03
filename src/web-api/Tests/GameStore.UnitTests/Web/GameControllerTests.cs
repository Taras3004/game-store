using GameStore.Core.Dto.Requests.Game;
using GameStore.Core.Dto.Responses;
using GameStore.Core.Interfaces.Services;
using GameStore.Models.Errors;
using GameStore.Models.Results;
using GameStore.UnitTests.Helpers.EntityTestBuilders;
using GameStore.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameStore.UnitTests.Web
{
    public class GamesControllerTests
    {
        private readonly Mock<IGameService> gameServiceMock = new();
        private readonly Mock<IOrderService> orderServiceMock = new();
        private readonly Mock<IPublisherService> publisherServiceMock = new();
        private readonly Mock<IGenreService> genreServiceMock = new();
        private readonly Mock<IPlatformService> platformServiceMock = new();
        private readonly Mock<IUserProvider> userProviderMock = new();

        private readonly GamesController controller;

        public GamesControllerTests()
        {
            controller = new GamesController(
                gameServiceMock.Object, 
                orderServiceMock.Object, 
                publisherServiceMock.Object, 
                genreServiceMock.Object, 
                platformServiceMock.Object,
                userProviderMock.Object);
        }

        [Fact]
        public async Task CreateGame_ShouldReturnCreatedAtActionResult_With201StatusCode()
        {
            string gameName = "New game";
            var request = new CreateGameRequest()
            {
                Game = GameTestBuilder.CreateValidGameDetailsBase(name: gameName),
                Publisher = "publisherId"
            };
            var expectedResponse = GameTestBuilder.CreateValidGameResponse(name: gameName);

            gameServiceMock
                .Setup(service => service.CreateGame(request, CancellationToken.None))
                .ReturnsAsync(expectedResponse);

            var result = await controller.CreateGame(request, CancellationToken.None);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal(nameof(GamesController.GetGameByIdAndIncrementViews), createdResult.ActionName);

            Assert.Equal(expectedResponse.Name, (createdResult.Value as GameResponse)!.Name);
        }

        [Fact]
        public async Task GetGameByKey_ShouldReturnOkObjectResult_With200StatusCode()
        {
            string gameKey = "test-game";
            var expectedResponse = GameTestBuilder.CreateValidGameResponse(key: gameKey);

            gameServiceMock
                .Setup(service => service.GetGameByKeyAndIncrementViews(gameKey, CancellationToken.None))
                .ReturnsAsync(expectedResponse);

            var result = await controller.GetGameByKeyAndIncrementViews(gameKey, CancellationToken.None);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResponse.Key, (okResult.Value as GameResponse)!.Key);
        }

        [Fact]
        public async Task UpdateGame_ShouldReturnNoContentResult_WhenServiceReturnsSuccess()
        {
            var request = new UpdateGameRequest()
            {
                Game = GameTestBuilder.CreateValidGameDetailsWithId(),
                Publisher = "publisherId"
            };

            gameServiceMock
                .Setup(service => service.UpdateGame(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success());

            var result = await controller.UpdateGame(request, CancellationToken.None);

            Assert.IsType<NoContentResult>(result);
            gameServiceMock.Verify(service => service.UpdateGame(request, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task DeleteGame_ShouldReturnNoContentResult_With204StatusCode()
        {
            string gameKey = "test-game";

            gameServiceMock
                .Setup(service => service.DeleteGame(gameKey, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success());

            var result = await controller.DeleteGame(gameKey, CancellationToken.None);

            Assert.IsType<NoContentResult>(result);
            gameServiceMock.Verify(service => service.DeleteGame(gameKey, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task UpdateGame_ShouldReturn404ProblemDetails_WhenGameIsNotFound()
        {
            var request = new UpdateGameRequest() 
            {
                Game = GameTestBuilder.CreateValidGameDetailsWithId(),
                Publisher = "publisherId"
            };

            gameServiceMock
                .Setup(service => service.UpdateGame(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure(GameErrors.NotFound));

            var result = await controller.UpdateGame(request, CancellationToken.None);

            var objectResult = Assert.IsType<ObjectResult>(result);

            var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);

            Assert.Equal(404, problemDetails.Status);
            Assert.Equal("Game.NotFound", problemDetails.Title);
        }

        [Fact]
        public async Task DownloadGameFile_ShouldReturnFileContentResult()
        {
            string gameKey = "test-game";
            var expectedFileResponse = GameTestBuilder.CreateValidGameFileResponse(
                fileContent: [1, 2, 3],
                contentType: "text/plain",
                fileName: "_test-game.txt"
            );

            gameServiceMock
                .Setup(service => service.DownloadGameFile(gameKey, CancellationToken.None))
                .ReturnsAsync(expectedFileResponse);

            var result = await controller.DownloadGameFile(gameKey, CancellationToken.None);

            var fileResult = Assert.IsType<FileContentResult>(result);

            Assert.Equal(expectedFileResponse.FileContent, fileResult.FileContents);
            Assert.Equal(expectedFileResponse.ContentType, fileResult.ContentType);
            Assert.Equal(expectedFileResponse.FileName, fileResult.FileDownloadName);
        }
    }
}
