using ContentGenerator.Models;
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
        private readonly IAudioGeneratorService _audioGeneratorService;
        public ContentGeneratorController(ILogger<ContentGeneratorController> logger, ITextGeneratorService textGeneratorService, IAudioGeneratorService audioGeneratorService)
        {
            _logger = logger;
            _textGeneratorService = textGeneratorService;
            _audioGeneratorService = audioGeneratorService;
        }

        [HttpGet("text/{question:alpha}")]
        public async Task<ActionResult<string?>> GenerateText(string question)
        {
            var response = await _textGeneratorService.GenerateText(question);
            _logger.LogInformation(response!.ToString());
            return response;
        }

        [HttpPost("audio")]
        public async Task<ActionResult<AudioData?>> GenerateAudio([FromForm] string text)
        {
            var response = await _audioGeneratorService.GenerateAudio(text);
            _logger.LogInformation(response!.ToString());
            //return response;
            return response;
        }
    }
}
