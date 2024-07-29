using EBlog.Domain;
using EBlog.Domain.Entities;
using EBlog.WebApi.Instruction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace EBlog.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly BlogService blogService;
        public BlogController(BlogService blogService)
        {
            this.blogService = blogService;
        }
        [HttpGet("{pid}")]
        public async Task<ActionResult> SelectArtcleById(int pid)
        {
            var art = await blogService.GetByIdAsync(pid);
            if (art == null)
            {
                return BadRequest("找不到该文章");
            }
            return Ok(art);
        }
    }
}
