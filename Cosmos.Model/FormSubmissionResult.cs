using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Cosmos.Model
{
    public class FormSubmissionResult
    {
        public int ItemId { get; set; }
        public int FormId { get; set; }
    }

    public class ItemForm
    {
        [Required] public int FormId { get; set; }
        [Required] public IFormFile ItemFormFile { get; set; }
    }
}
