using System.IO.Pipelines;
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
        private readonly IContentService _contentService;

        public ContentGeneratorController(ILogger<ContentGeneratorController> logger, ITextGeneratorService textGeneratorService, IAudioGeneratorService audioGeneratorService, IContentService contentService)
        {
            _logger = logger;
            _textGeneratorService = textGeneratorService;
            _audioGeneratorService = audioGeneratorService;
            _contentService = contentService;
        }

        [HttpGet("text/{question:alpha}")]
        public async Task<ActionResult<string?>> GenerateText(string question)
        {
            var response = await _textGeneratorService.GenerateText(question);
            _logger.LogInformation(response!.ToString());
            return response;
        }

        [HttpPost("audio")]
        public async Task<ActionResult<AudioData?>> GenerateAudio(TextRequest textRequest)
        {
            var response = await _audioGeneratorService.GenerateAudio(textRequest.Text);
            return response;
        }

        [HttpPost("content")]
        public async Task<ActionResult<Content?>> GenerateContent(TextRequest textRequest)
        {
            Content content = new Content();
            content.Text = await _textGeneratorService.GenerateText(textRequest.Text);
            content.AudioData = await _audioGeneratorService.GenerateAudio(content.Text);
        	content.Tongue = textRequest.Tongue;
            content.Type = textRequest.Type;
            await _contentService.CreateContent(content);
            return content;
        }
    }
}
