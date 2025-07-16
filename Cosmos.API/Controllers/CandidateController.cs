using Cosmos.Api.HubConfig;
using Cosmos.Api.Services;
using Cosmos.Common;
using Cosmos.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cosmos.Api.Controllers
{
    [ApiExplorerSettings(GroupName = "cosmosdb")]
    [Route("api/[controller]")]
    public class CandidateController : ControllerBase
    {
        private readonly ILogger<CandidateController> _logger;
        private readonly IHubContext<CandidateHub> _hub;
        readonly ICandidateService _candidateService;

        public CandidateController(ILogger<CandidateController> logger, IHubContext<CandidateHub> hub, ICandidateService candidateService)
        {
            _logger = logger;
            _hub = hub;
            _candidateService = candidateService;
        }

        [HttpGet]
        public async Task<IActionResult> SendCandidates()
        {
            var candidates = await _candidateService.GetAsync();
            await _hub.Clients.All.SendAsync("sendCandidates", candidates);
            return Ok(new { Message = "sendCandidates success" });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> All()
        {
            var candidates = await _candidateService.GetAsync();
            return new ObjectResult(candidates);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Get(string id, [FromQuery] ResidentialType? residentialType = null)
        {
            try
            {
                var candidate = await _candidateService.GetAsync(id);
                return new ObjectResult(candidate);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Item {id} could not be found: {ex.Message}");
                _logger.LogError($"ResidentialType:{residentialType}");

                return new NotFoundObjectResult(id);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] Candidate candidate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _candidateService.CreateAsync(candidate);

            return new ObjectResult(result);
        }

        [HttpPost("[action]/Collection")]
        public async Task<IActionResult> Create([FromBody] IEnumerable<Candidate> candidates)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _candidateService.CreateAsync(candidates);
            return new ObjectResult(result);
        }

        [HttpPost("[action]/{count}/{saveToDatabase}")]
        public async Task<IActionResult> Generate(int count, bool saveToDatabase = false)
        {
            var result = await _candidateService.CreateAsync(count, saveToDatabase);
            return new ObjectResult(result);
        }

        [HttpPut("[action]/{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] Candidate candidate)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != candidate.Id)
            {
                return BadRequest(ModelState);
            }

            var original = await _candidateService.GetAsync(id);

            if (original == null)
            {
                return NotFound(id);
            }

            var result = await _candidateService.UpdateAsync(candidate.Id, original.LastName, candidate);

            return new ObjectResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var result = await _candidateService.DeleteAsync(id);

            return Ok(result);
        }

        [HttpDelete("All")]
        public async Task<IActionResult> Delete()
        {
            var candidates = await _candidateService.GetAsync();

            foreach (var candidate in candidates)
            {
                await _candidateService.DeleteAsync(candidate.Id);
            }

            // Insert a dummy candidate to allow for a change feed trigger
            await _candidateService.CreateDummyAsync();

            return Ok(new { Message = $"{candidates.Count()} candidates have been deleted" });
        }
    }
}
