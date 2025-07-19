using Cosmos.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Cosmos.FunctionsApp
{
    public class CosmosWebJob
    {
        private readonly ILogger _logger;

        public CosmosWebJob(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CosmosWebJob>();
        }

        [Function("CreateMovie")]
        [CosmosDBOutput(
            "%DatabaseName%",
            "%ContainerName%",
            Connection = "CosmosDBConnectionString")]
        public async Task<object> Create(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "movie")] HttpRequest req,
            [CosmosDBInput(
                databaseName: "%DatabaseName%",
                containerName: "%ContainerName%",
                Connection = "CosmosDBConnectionString")]
            IReadOnlyList<Movie> input)
        {
            var request = await new StreamReader(req.Body).ReadToEndAsync();
            var movie = JsonConvert.DeserializeObject<Movie>(request)!;

            _logger.LogInformation($"Movie with ID '{movie.Id}' created successfully");

            // Cosmos Output
            return input.Select(p => new {
                movieID = movie.MovieID,
                title = movie.Title,
                releaseDate = movie.ReleaseDate,
                price = movie.Price,
                poster = movie.Poster,
                year = movie.Year,
                genre = movie.Genre,
                movieRatings = movie.MovieRatings.Select(r => new {
                    rated = r.Rated,
                    language = r.Language,
                    metascore = r.Metascore,
                    rating = r.Rating,
                    votes = r.Votes
                }),
                isActive = movie.IsActive,
                id = movie.Id,
                type = movie.Type
            });
        }

        [Function("UpdateMovie")]
        [CosmosDBOutput(
            "%DatabaseName%",
            "%ContainerName%",
            Connection = "CosmosDBConnectionString")]
        public async Task<object> Update(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "movie/{id}")] HttpRequest req, string id,
            [CosmosDBInput(
                databaseName: "%DatabaseName%",
                containerName: "%ContainerName%",
                Connection = "CosmosDBConnectionString")]
            IReadOnlyList<Movie> input)
        {
            var request = await new StreamReader(req.Body).ReadToEndAsync();
            var movie = JsonConvert.DeserializeObject<Movie>(request)!;

            if (input == null || !input.Any(o => o.Id == id)) return null!;

            _logger.LogInformation($"Movie with ID '{movie.Id}' updated successfully");

            // Cosmos Output
            return input
                .Where(o => o.Id == id)
                .Select(o => new
                {
                    movieID = movie.MovieID,
                    title = movie.Title,
                    releaseDate = movie.ReleaseDate,
                    price = movie.Price,
                    poster = movie.Poster,
                    year = movie.Year,
                    genre = movie.Genre,
                    movieRatings = movie.MovieRatings.Select(r => new {
                        rated = r.Rated,
                        language = r.Language,
                        metascore = r.Metascore,
                        rating = r.Rating,
                        votes = r.Votes
                    }),
                    isActive = movie.IsActive,
                    id = o.Id,
                    type = o.Type
                });
        }
    }
}
