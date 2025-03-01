using ContentGenerator.Models;
using ContentGenerator.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IContentService _contentService;
        private readonly ILogger<ContentController> _logger;
        private readonly ITextGeneratorService _textGeneratorService;
        public ContentController(IContentService contentService, ILogger<ContentController> logger, ITextGeneratorService textGeneratorService)
        {
            _contentService =  contentService;
            _logger = logger;
            _textGeneratorService = textGeneratorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Content>>> GetContents()
        {
            var contents = await _contentService.GetAllContents();
            return Ok(contents);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Content>> GetContent(string id)
        {
            _logger.LogInformation(id);
            var content = await _contentService.GetContent(id);
            if (content == null)
            {
                return NotFound();
            }
            return Ok(content);
            /*
            return new Content{
                Id = "fakeId",
                Text = "Hello World",
                Tongue = Tongue.Deutsch,
                Type = ContentType.Presentation,
                AudioData = new AudioData {
                    AudioFile = "home/audios/hello",
                    AudioLengthInSeconds = 2.5,
                    WordDurations = [
                        new WordDuration{Word="hello", StartMs = 0.0, EndMs = 1.1},
                        new WordDuration{Word="world", StartMs = 1.3, EndMs = 2.1}
                    ]
                }
            };
            */
        }

        [HttpPost]
        public async Task<ActionResult<Content>> CreateContent(Content content)
        {
            await _contentService.CreateContent(content);
            _logger.LogInformation(content.Id);
            return CreatedAtAction(nameof(GetContent), new {id = content.Id}, content);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateContent(string id, Content content)
        {
            if (id != content.Id)
            {
                return BadRequest();
            }
            var updatedContent = await _contentService.UpdateContent(id, content);
            if (!updatedContent)
            {
                return NotFound();
            }
            return Ok(content);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteContent(string id)
        {
            await _contentService.DeleteContent(id);
            return NoContent();
        }
    }
}
