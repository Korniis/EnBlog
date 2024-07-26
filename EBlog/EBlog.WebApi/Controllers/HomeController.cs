using EBlog.Domain.Entities;
using EBlog.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EBlog.WebApi.Instruction;

namespace EBlog.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly BlogService blogService;

        public HomeController(BlogService blogService)
        {
            this.blogService = blogService;
        }

        [HttpGet]
        public async Task< ActionResult> GetPageInfo()
        {
            var page = await blogService.GetAllAsync();
            page.Select(article => new
            {
                Id = article.Id,
                Title = article.Title,
                num = article.ReadCount,
                datetime = article.CreatedDate,
                Content = article.Content.Length > 100 ? article.Content.Substring(0, 100) : article.Content
            }).ToList();

            return Ok(page);
        }

    }
}
