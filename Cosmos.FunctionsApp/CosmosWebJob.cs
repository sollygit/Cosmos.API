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

        [Function("CreateCandidate")]
        [CosmosDBOutput(
            "%DatabaseName%",
            "%ContainerName%",
            Connection = "CosmosDBConnectionString")]
        public async Task<object> Create(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "candidate")] HttpRequest req,
            [CosmosDBInput(
                databaseName: "%DatabaseName%",
                containerName: "%ContainerName%",
                Connection = "CosmosDBConnectionString")]
            IReadOnlyList<Candidate> input)
        {
            var request = await new StreamReader(req.Body).ReadToEndAsync();
            var candidate = JsonConvert.DeserializeObject<Candidate>(request)!;

            _logger.LogInformation("Create:{FullName}, ID:{Id}", candidate.FullName, candidate.Id);

            // Cosmos Output
            return input.Select(p => new { 
                id = candidate.Id,
                firstName = candidate.FirstName,
                lastName = candidate.LastName,
                email = candidate.Email,
                balance = candidate.Balance,
                points = candidate.Points,
                registrationDate = candidate.RegistrationDate,
                isActive = candidate.IsActive,
                technologies = candidate.Technologies
            });
        }

        [Function("UpdateCandidate")]
        [CosmosDBOutput(
            "%DatabaseName%",
            "%ContainerName%",
            Connection = "CosmosDBConnectionString")]
        public async Task<object> Update(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "candidate/{id}")] HttpRequest req, Guid id,
            [CosmosDBInput(
                databaseName: "%DatabaseName%",
                containerName: "%ContainerName%",
                Connection = "CosmosDBConnectionString")]
            IReadOnlyList<Candidate> input)
        {
            var request = await new StreamReader(req.Body).ReadToEndAsync();
            var candidate = JsonConvert.DeserializeObject<Candidate>(request)!;

            if (input == null || !input.Any(o => o.Id == id.ToString())) return null!;

            _logger.LogInformation("Update:{FullName}, ID:{id}", candidate.FullName, id);

            // Cosmos Output
            return input
                .Where(o => o.Id == id.ToString())
                .Select(o => new
                {
                    id = o.Id,
                    lastName = candidate.LastName,
                    firstName = candidate.FirstName,
                    email = candidate.Email,
                    balance = candidate.Balance,
                    points = candidate.Points,
                    registrationDate = candidate.RegistrationDate,
                    isActive = candidate.IsActive,
                    technologies = candidate.Technologies
                });
        }
    }
}
