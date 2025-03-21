using ContentGenerator.Models;
using ContentGenerator.Models.Authentication;
using ContentGenerator.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContentGenerator.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = $"{UserRoles.User},{UserRoles.VipUser},{UserRoles.Administrator}")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IContentService _contentService;
        private readonly ILogger<ContentController> _logger;
        public ContentController(IContentService contentService, ILogger<ContentController> logger)
        {
            _contentService =  contentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Content>>> GetContents()
        {
            var contents = await _contentService.GetAllContents();
            List<Content> myContents = [];
            foreach (var content in contents)
            {
                if (content.AudioData.WordDurations.Count > 0)
                {
                    myContents.Add(content);
                }
            }
            return Ok(myContents);
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
