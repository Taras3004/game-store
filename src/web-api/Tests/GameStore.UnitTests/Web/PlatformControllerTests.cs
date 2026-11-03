using GameStore.Core.Dto.Requests.Platform;
using GameStore.Core.Dto.Responses;
using GameStore.Core.Interfaces.Services;
using GameStore.Models.Results;
using GameStore.UnitTests.Helpers.EntityTestBuilders;
using GameStore.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameStore.UnitTests.Web
{
    public class PlatformsControllerTests
    {
        private readonly Mock<IPlatformService> platformServiceMock = new();
        private readonly Mock<IGameService> gameServiceMock = new();
        private readonly PlatformsController controller;

        public PlatformsControllerTests()
        {
            controller = new PlatformsController(
                platformServiceMock.Object, 
                gameServiceMock.Object);
        }

        [Fact]
        public async Task CreatePlatform_ShouldReturnCreatedAtActionResult_With201StatusCode()
        {
            var request = new CreatePlatformRequest
            {
                Platform = PlatformTestBuilder.CreateValidPlatformDetailsBase()
            };
            var expectedResponse = PlatformTestBuilder.CreateValidPlatformResponse();

            platformServiceMock
                .Setup(service => service.CreatePlatform(request, CancellationToken.None))
                .ReturnsAsync(expectedResponse);

            var result = await controller.CreatePlatform(request, CancellationToken.None);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(PlatformsController.GetPlatform), createdResult.ActionName);
            Assert.Equal(expectedResponse, createdResult.Value);
        }

        [Fact]
        public async Task GetPlatform_ShouldReturnOkObjectResult_With200StatusCode()
        {
            var platformId = Guid.NewGuid();
            var expectedResponse = PlatformTestBuilder.CreateValidPlatformResponse(id: platformId);

            platformServiceMock
                .Setup(service => service.GetPlatform(platformId, CancellationToken.None))
                .ReturnsAsync(expectedResponse);

            var result = await controller.GetPlatform(platformId, CancellationToken.None);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResponse, okResult.Value);
        }

        [Fact]
        public async Task GetAllPlatforms_ShouldReturnOkObjectResult_WithCollection()
        {
            var expectedPlatforms = new List<PlatformResponse>
            {
                PlatformTestBuilder.CreateValidPlatformResponse(),
                PlatformTestBuilder.CreateValidPlatformResponse()
            };

            platformServiceMock
                .Setup(service => service.GetAllPlatforms(CancellationToken.None))
                .ReturnsAsync(expectedPlatforms);

            var result = await controller.GetAllPlatforms(CancellationToken.None);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedPlatforms, okResult.Value);
        }

        [Fact]
        public async Task UpdatePlatform_ShouldReturnNoContentResult_With204StatusCode()
        {
            var request = new UpdatePlatformRequest()
            {
                Platform = PlatformTestBuilder.CreateValidPlatformDetailsWithId()
            };

            platformServiceMock
                .Setup(service => service.UpdatePlatform(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success());

            var result = await controller.UpdatePlatform(request, CancellationToken.None);

            Assert.IsType<NoContentResult>(result);
            platformServiceMock.Verify(service => service.UpdatePlatform(request, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task DeletePlatform_ShouldReturnNoContentResult_With204StatusCode()
        {
            var platformId = Guid.NewGuid();

            platformServiceMock
                .Setup(service => service.DeletePlatform(platformId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success());

            var result = await controller.DeletePlatform(platformId, CancellationToken.None);

            Assert.IsType<NoContentResult>(result);
            platformServiceMock.Verify(service => service.DeletePlatform(platformId, CancellationToken.None), Times.Once);
        }
    }
}