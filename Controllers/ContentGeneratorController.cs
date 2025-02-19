using ContentGenerator.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentGeneratorController : ControllerBase
    {
        private readonly ILogger<ContentGeneratorController> _logger;
        private readonly ITextGeneratorService _textGeneratorService;
        public ContentGeneratorController(ILogger<ContentGeneratorController> logger, ITextGeneratorService textGeneratorService)
        {
            _logger = logger;
            _textGeneratorService = textGeneratorService;
        }

        [HttpGet("text/{question:alpha}")]
        public async Task<ActionResult<string?>> GenerateContent(string question)
        {
            var response = await _textGeneratorService.GenerateText(question);
            _logger.LogInformation(response!.ToString());
            return response;
        }
    }
}
