using GameStore.Core.Dto.Requests.Genre;
using GameStore.Core.Dto.Responses;
using GameStore.Core.Interfaces.Services;
using GameStore.Models.Results;
using GameStore.UnitTests.Helpers.EntityTestBuilders;
using GameStore.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameStore.UnitTests.Web
{
    public class GenresControllerTests
    {
        private readonly Mock<IGenreService> genreServiceMock = new();
        private readonly Mock<IGameService> gameServiceMock = new();
        private readonly GenresController controller;

        public GenresControllerTests()
        {
            controller = new GenresController(
                genreServiceMock.Object,
                gameServiceMock.Object);
        }

        [Fact]
        public async Task CreateGenre_ShouldReturnCreatedAtActionResult_With201StatusCode()
        {
            var request = new CreateGenreRequest()
            { 
                Genre = GenreTestBuilder.CreateValidGenreDetailsBase()
            };
            var expectedResponse = GenreTestBuilder.CreateValidGenreResponse();

            genreServiceMock
                .Setup(service => service.CreateGenre(request, CancellationToken.None))
                .ReturnsAsync(expectedResponse);

            var result = await controller.CreateGenre(request, CancellationToken.None);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(GenresController.GetGenreById), createdResult.ActionName);
            Assert.Equal(expectedResponse, createdResult.Value);
        }

        [Fact]
        public async Task GetGenre_ShouldReturnOkObjectResult_With200StatusCode()
        {
            var genreId = "orderId";
            var expectedResponse = GenreTestBuilder.CreateValidGenreResponse(id: genreId);

            genreServiceMock
                .Setup(service => service.GetGenreById(genreId, CancellationToken.None))
                .ReturnsAsync(expectedResponse);

            var result = await controller.GetGenreById(genreId, CancellationToken.None);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResponse, okResult.Value);
        }

        [Fact]
        public async Task GetAllGenres_ShouldReturnOkObjectResult_WithCollection()
        {
            var expectedGenres = new List<GenreResponse>
            {
                GenreTestBuilder.CreateValidGenreResponse(),
                GenreTestBuilder.CreateValidGenreResponse()
            };

            genreServiceMock
                .Setup(service => service.GetAllGenres(CancellationToken.None))
                .ReturnsAsync(expectedGenres);

            var result = await controller.GetAllGenres(CancellationToken.None);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedGenres, okResult.Value);
        }

        [Fact]
        public async Task UpdateGenre_ShouldReturnNoContentResult_With204StatusCode()
        {
            var request = new UpdateGenreRequest()
            {
                Genre = GenreTestBuilder.CreateValidGenreDetailsWithId()
            };

            genreServiceMock
                .Setup(service => service.UpdateGenre(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success());

            var result = await controller.UpdateGenre(request, CancellationToken.None);

            Assert.IsType<NoContentResult>(result);
            genreServiceMock.Verify(service => service.UpdateGenre(request, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task DeleteGenre_ShouldReturnNoContentResult_With204StatusCode()
        {
            var genreId = "orderId";

            genreServiceMock
                .Setup(service => service.DeleteGenre(genreId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success());

            var result = await controller.DeleteGenre(genreId, CancellationToken.None);

            Assert.IsType<NoContentResult>(result);
            genreServiceMock.Verify(service => service.DeleteGenre(genreId, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task SetParentGenre_ShouldReturnNoContentResult_With204StatusCode()
        {
            var genreId = Guid.NewGuid();
            var parentId = Guid.NewGuid();

            genreServiceMock
                .Setup(service => service.AddParentGenre(genreId, parentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success());

            var result = await controller.SetParentGenre(genreId, parentId, CancellationToken.None);

            Assert.IsType<NoContentResult>(result);
            genreServiceMock.Verify(service => service.AddParentGenre(genreId, parentId, CancellationToken.None), Times.Once);
        }
    }
}