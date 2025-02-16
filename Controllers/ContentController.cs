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
        private readonly ContentService _contentService;
        public ContentController()
        {
            _contentService = new ContentService();
        }

        [HttpGet]
        public async Task<ActionResult<List<Content>>> GetPosts()
        {
            var contents = await _contentService.GetAllContents();
            return Ok(contents);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Content>> GetContent(int id)
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
            await _contentService.CreateContent(content);
            return CreatedAtAction(nameof(GetContent), new {id = content.Id}, content);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateContent(int id, Content content)
        {
            if (id != content.Id)
            {
                return BadRequest();
            }
            var updatedContent = await _contentService.UpdateContent(id, content);
            if (updatedContent == null)
            {
                return NotFound();
            }
            return Ok(content);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteContent(int id)
        {
            var post = await _contentService.GetContent(id);
        if (post == null)
        {
            return NotFound();
        }

        await _contentService.DeleteContent(id);
        return NoContent();
        }
    }
}
