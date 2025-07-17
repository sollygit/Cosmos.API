using Cosmos.Api.HubConfig;
using Cosmos.Api.Services;
using Cosmos.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cosmos.Api.Controllers
{
    [ApiExplorerSettings(GroupName = "cosmosdb")]
    [Route("api/[controller]")]
    public class ChartController : ControllerBase
    {
        private readonly IHubContext<ChartHub> _hub;
        readonly IMovieService _movieService;

        public ChartController(IHubContext<ChartHub> hub, IMovieService movieService)
        {
            _hub = hub;
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> TransferChartData()
        {
            var chartData = await GetChartData();
            await _hub.Clients.All.SendAsync("transferChartData", chartData);
            return Ok(new { Message = "transferChartData success" });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Data()
        {
            var chartData = await GetChartData();
            return new ObjectResult(chartData);
        }

        async Task<IEnumerable<Chart>> GetChartData()
        {
            var movies = await _movieService.GetAsync();
            var genereList = movies.Select(o => o.Genre).Where(o => o != null);
            var lstChart = new List<Chart>();
            var dictionary = new Dictionary<string, int>();

            foreach (var arr in genereList)
            {
                arr.ToList().ForEach(key => {
                    if (!dictionary.TryGetValue(key, out int value))
                    {
                        dictionary.Add(key, 1);
                    }
                    else
                    {
                        dictionary[key] = ++value;
                    }
                });
            }

            dictionary.Keys.ToList().ForEach(key => {
                lstChart.Add(new Chart { Label = key, Data = [dictionary[key]] });
            });

            return lstChart.OrderBy(o => o.Label);
        }
    }
}