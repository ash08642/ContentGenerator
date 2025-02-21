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
        public async Task<ActionResult<List<Content>>> GetPosts()
        {
            var contents = await _contentService.GetAllContents();
            return Ok(contents);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Content>> GetContent(Guid id)
        {
            var content = await _contentService.GetContent(id);
            if (content == null)
            {
                return NotFound();
            }
            return Ok(content);
        }

        [HttpPost]
        public async Task<ActionResult<Content>> CreateContent(Content content)
        {
            bool res = await _contentService.CreateContent(content);
            return CreatedAtAction(nameof(GetContent), new {id = content.Id}, content);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateContent(Guid id, Content content)
        {
            if (id != content.Id)
            {
                return BadRequest();
            }
            var updatedContent = await _contentService.UpdateContent(id, content);
            if (updatedContent == false)
            {
                return NotFound();
            }
            return Ok(content);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteContent(Guid id)
        {
            await _contentService.DeleteContent(id);
            return NoContent();
        }
    }
}
