using ContentGenerator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        [HttpGet]
        public ActionResult<Content> GetTestContent()
        {
            return new Content() {Id = 1, Text = "Hellol", AudioPath= "file/checkList"};
        }
    }
}
