using Cosmos.Model;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Cosmos.FunctionsApp
{
    public class ChangeFeedWebJob
    {
        ILogger _logger;
        readonly HttpClient _client;

        public ChangeFeedWebJob(ILoggerFactory loggerFactory, IHttpClientFactory httpClientFactory)
        {
            _logger = loggerFactory.CreateLogger<ChangeFeedWebJob>();
            _client = httpClientFactory.CreateClient();
        }

        [Function("ChangeFeedWebJob")]
        public async Task Run([CosmosDBTrigger(
            databaseName: "%DatabaseName%",
            containerName: "%ContainerName%",
            Connection = "CosmosDBConnectionString",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)] IReadOnlyList<Movie> movies)
        {
            if (movies != null && movies.Count > 0)
            {
                _logger.LogInformation($"Movie '{movies[0].Title}' has been modified. Documents Count:{movies.Count}.");

                var _cosmosApi = Environment.GetEnvironmentVariable("CosmosApi")!;
                var movieResponse = await _client.GetAsync($"{_cosmosApi}/api/movie"); // Invoke sendMovies
                var chartResponse = await _client.GetAsync($"{_cosmosApi}/api/chart"); // Invoke transferChartData

                _logger.LogInformation($"SignalR Movie Response: {movieResponse.StatusCode}");
                _logger.LogInformation($"SignalR Chart Response: {chartResponse.StatusCode}");

                await Task.FromResult(movieResponse.StatusCode);
            }
        }
    }
}
