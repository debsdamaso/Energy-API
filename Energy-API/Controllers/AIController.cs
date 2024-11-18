using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Energy_API.Services;
using Energy_API.DTOs;

namespace Energy_API.Controllers
{
    [ApiController]
    [Route("api/ai")]
    public class AIController : ControllerBase
    {
        private readonly HuggingFaceService _huggingFaceService;

        public AIController(HuggingFaceService huggingFaceService)
        {
            _huggingFaceService = huggingFaceService;
        }

        [HttpPost("generate-text")]
        public async Task<IActionResult> GenerateText([FromBody] GenerateTextRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
            {
                return BadRequest("O prompt não pode estar vazio.");
            }

            var generatedText = await _huggingFaceService.GenerateTextAsync(request.Prompt);
            return Ok(new { Text = generatedText });
        }
    }
}
