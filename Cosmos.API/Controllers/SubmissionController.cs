using Cosmos.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cosmos.API.Controllers
{
    [ApiExplorerSettings(GroupName = "forms")]
    [Route("api/[controller]")]
    public class SubmissionController : ControllerBase
    {
        private readonly ILogger<SubmissionController> _logger;

        public SubmissionController(ILogger<SubmissionController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id:int}/Forms/{formId:int}")]
        [ProducesResponseType(typeof(FormSubmissionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<FormSubmissionResult> ViewForm(int id, int formId)
        {
            _logger.LogInformation($"Viewing form {formId} for item ID={id}");
            await Task.Delay(1000);
            return new FormSubmissionResult { FormId = formId, ItemId = id };
        }

        [HttpPost("{id:int}/Forms")]
        [ProducesResponseType(typeof(FormSubmissionResult), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FormSubmissionResult>> SubmitForm(int id, [FromForm] ItemForm itemForm)
        {
            _logger.LogInformation($"Validating the form#{itemForm.FormId} for Item ID={id}");
            _logger.LogInformation($"Saving file [{itemForm.ItemFormFile.FileName}]");
            await Task.Delay(1500);
            _logger.LogInformation("File saved.");
            var result = new FormSubmissionResult { FormId = itemForm.FormId, ItemId = id };
            return CreatedAtAction(nameof(ViewForm), new { id, itemForm.FormId }, result);
        }

        [HttpDelete("{id:int}/Forms/{formId:int}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<bool> Delete(int id, int formId)
        {
            _logger.LogInformation($"Deleting the form#{formId} for item ID=[{id}]");
            await Task.Delay(1500);
            return true;
        }
    }
}
