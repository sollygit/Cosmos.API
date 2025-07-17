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
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        private readonly IHubContext<MovieHub> _hub;
        readonly IMovieService _movieService;

        public MovieController(ILogger<MovieController> logger, IHubContext<MovieHub> hub, IMovieService movieService)
        {
            _logger = logger;
            _hub = hub;
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> SendMovies()
        {
            var items = await _movieService.GetAsync();
            await _hub.Clients.All.SendAsync("sendMovies", items);
            return Ok(new { Message = "SendMovies success" });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> All()
        {
            var items = await _movieService.GetAsync();
            return new ObjectResult(items);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Get(string id, [FromQuery] ResidentialType? residentialType = null)
        {
            try
            {
                var item = await _movieService.GetAsync(id);
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Item {id} could not be found: {ex.Message}");
                _logger.LogError($"ResidentialType:{residentialType}");
                return NotFound(id);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _movieService.CreateAsync(movie);

            return new ObjectResult(result);
        }

        [HttpPost("[action]/Collection")]
        public async Task<IActionResult> Create([FromBody] IEnumerable<Movie> movies)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _movieService.CreateAsync(movies);
            return new ObjectResult(result);
        }

        [HttpPost("[action]/{count}/{saveToDatabase}")]
        public async Task<IActionResult> Generate(int count, bool saveToDatabase = false)
        {
            var result = await _movieService.CreateAsync(count, saveToDatabase);
            return new ObjectResult(result);
        }

        [HttpPut("[action]/{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] Movie movie)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != movie.Id)
            {
                return BadRequest(ModelState);
            }

            var original = await _movieService.GetAsync(id);

            if (original == null)
            {
                return NotFound(id);
            }

            var result = await _movieService.UpdateAsync(movie.Id, movie);

            return new ObjectResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            try
            {
                var item = await _movieService.DeleteAsync(id);
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Item {id} could not be found: {ex.Message}");
                return new NotFoundObjectResult(id);
            }
        }

        [HttpDelete("All")]
        public async Task<IActionResult> Delete()
        {
            var items = await _movieService.GetAsync();

            foreach (var item in items)
            {
                await _movieService.DeleteAsync(item.Id);
            }

            // Insert a dummy item to allow for a change feed trigger
            await _movieService.CreateDummyAsync();

            return Ok(new { Message = $"{items.Count()} items have been deleted" });
        }
    }
}
