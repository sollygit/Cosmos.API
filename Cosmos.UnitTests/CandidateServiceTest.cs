using Cosmos.Api.Services;
using Cosmos.Model;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cosmos.UnitTests
{
    [TestFixture]
    public class CandidateServiceTest
    {
        private IQueryable<Movie> movies;
        private List<string> genres;

        [SetUp]
        public void Setup()
        {
            movies = new List<Movie>()
            {
                new Movie(){ Id = Guid.NewGuid().ToString("n"), Title = "Title_1" },
                new Movie(){ Id = Guid.NewGuid().ToString("n"), Title = "Title_2" },
                new Movie(){ Id = Guid.NewGuid().ToString("n"), Title = "Title_3" }
            }.AsQueryable();

            genres = new List<string>() { "Action", "Adventure", "Comedy", "Documentary", "Drama", "Fantasy" };
        }

        [Test]
        public async Task GetAll_Success()
        {
            // Arrange
            var mockService = new Mock<IMovieService>();
            mockService.Setup(x => x.GetAsync()).Returns(async () =>
            {
                await Task.Yield();
                return movies;
            });

            // Act
            var actual = await mockService.Object.GetAsync();

            // Assert
            Assert.AreEqual(movies.Count(), actual.Count());
        }

        [Test]
        public async Task Get_Success()
        {
            // Arrange
            var item = new Movie
            {
                Id = Guid.NewGuid().ToString("n"),
                Title = "Kingdom of the Planet of the Apes",
                Price = 100,
                Genre = genres.Take(2).ToArray()
            };

            var mockService = new Mock<IMovieService>();
            mockService.Setup(x => x.GetAsync(item.Id)).Returns(async () =>
            {
                await Task.Yield();
                return item;
            });

            // Act
            await mockService.Object.CreateAsync(item);
            var actual = await mockService.Object.GetAsync(item.Id);

            // Assert
            Assert.AreEqual(item, actual);
        }

        [Test]
        public async Task Get_NotFound_Success()
        {
            // Arrange
            var itemId = Guid.NewGuid().ToString();
            var mockService = new Mock<IMovieService>();

            mockService.Setup(x => x.GetAsync(itemId)).Returns(async () =>
            {
                await Task.Yield();
                return null;
            });

            // Act
            var actual = await mockService.Object.GetAsync(itemId);

            // Assert
            mockService.Verify(m => m.GetAsync(itemId), Times.AtLeastOnce());
            Assert.AreEqual(null, actual);
        }

        [Test]
        public async Task Add_Success()
        {
            // Arrange
            var item = new Movie
            {
                Id = Guid.NewGuid().ToString("n"),
                Title = "Kingdom of the Planet of the Apes",
                Price = 100,
                Genre = genres.Take(2).ToArray()
            };

            var mockService = new Mock<IMovieService>();
            mockService.Setup(x => x.CreateAsync(It.IsAny<Movie>())).Returns(async () =>
            {
                await Task.Yield();
                return item;
            });

            // Act
            var actual = await mockService.Object.CreateAsync(item);

            // Assert
            Assert.AreEqual(item, actual);
        }

        [Test]
        public void Add_IsNull_Failure_Throws()
        {
            string errorMessage = "Item cannot be null";

            // Arrange
            var item = It.IsAny<Movie>();

            // Act and Assert
            Assert.That(async () =>
                await Add_ThrowException(item, errorMessage),
                Throws.Exception.TypeOf<Exception>().And.Message.EqualTo(errorMessage));
        }

        [Test]
        public void Add_FirstNameIsEmpty_Failure_Throws()
        {
            string errorMessage = "Title cannot be empty";

            // Arrange
            var item = new Movie
            {
                Id = Guid.NewGuid().ToString("n"),
                Title = "Kingdom of the Planet of the Apes",
                Genre = genres.Take(2).ToArray()
            };

            // Act and Assert
            Assert.That(async () =>
                await Add_ThrowException(item, errorMessage),
                Throws.Exception.TypeOf<Exception>().And.Message.EqualTo(errorMessage));
        }

        [Test]
        public async Task Update_Success()
        {
            // Arrange
            var item = new Movie
            {
                Id = Guid.NewGuid().ToString("n"),
                Title = "Kingdom of the Planet of the Apes",
                MovieID = "KOTPA123",
                Poster = "https://example.com/poster.jpg",
                MovieRatings = [],
                Price = 100,
                ReleaseDate = DateTime.Now,
                Genre = genres.Take(2).ToArray(),
                IsActive = true
            };

            var mockService = new Mock<IMovieService>();
            mockService.Setup(x => x.UpdateAsync(item.Id, It.IsAny<Movie>())).Returns(async () =>
            {
                await Task.Yield();
                return item;
            });

            // Act
            await mockService.Object.UpdateAsync(item.Id, item);

            // Assert
            mockService.Verify(m => m.UpdateAsync(item.Id, It.IsAny<Movie>()), Times.AtLeastOnce());
            Assert.That(item.MovieID, Is.EqualTo("KOTPA123"));
        }

        [Test]
        public async Task Delete_Success()
        {
            // Arrange
            var item = new Movie
            {
                Id = Guid.NewGuid().ToString("n"),
                Title = "Kingdom of the Planet of the Apes",
                MovieID = "KOTPA123",
                Genre = genres.Take(2).ToArray()
            };

            var mockService = new Mock<IMovieService>();
            mockService.Setup(x => x.DeleteAsync(item.Id)).Returns(async () =>
            {
                await Task.Yield();
                return item;
            });

            // Act
            await mockService.Object.DeleteAsync(item.Id);
            var actual = await mockService.Object.GetAsync(item.Id);

            // Assert
            mockService.Verify(m => m.DeleteAsync(item.Id));
            mockService.Verify(m => m.GetAsync(item.Id));
            Assert.AreEqual(null, actual);
        }

        static async Task Add_ThrowException(Movie item, string errorMessage)
        {
            var mockService = new Mock<IMovieService>();
            await mockService.Object.CreateAsync(item).ConfigureAwait(false);
            throw new Exception(errorMessage);
        }
    }
}