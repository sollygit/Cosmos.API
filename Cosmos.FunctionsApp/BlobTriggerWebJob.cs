using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Cosmos.FunctionsApp
{
    public class BlobTriggerWebJob
    {
        readonly string _cosmosApi = Environment.GetEnvironmentVariable("CosmosApi")!;
        readonly ILogger<BlobTriggerWebJob> _logger;
        readonly HttpClient _client;

        public BlobTriggerWebJob(ILogger<BlobTriggerWebJob> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient();
        }

        [Function("BlobTriggerWebJob")]
        public async Task Run([BlobTrigger(blobPath: "%BlobPath%/{name}", Connection = "StorageConnectionString")] Stream stream, string name)
        {
            _logger.LogInformation("C# BlobTriggerWebJob function Processed:\n{name}", name);

            try
            {
                var jsonRequest = await new StreamReader(stream).ReadToEndAsync();
                var jsonContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var uriBuilder = new UriBuilder($"{_cosmosApi}/api/Candidate/Create/Collection");
                var response = await _client.PostAsync(uriBuilder.Uri, jsonContent);
                var content = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"StatusCode: '{response.StatusCode}'\nContent: '{content}'");
            }
            catch (Exception ex)
            {
                _logger.LogError("JSON content error:{Message}", ex.Message);
            }
        }
    }
}
