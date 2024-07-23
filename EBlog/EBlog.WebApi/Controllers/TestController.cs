using EBlog.Domain;
using EBlog.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EBlog.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly MyDbContext myDbContext;

        public TestController(MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;
        }

        [HttpGet]
        public ActionResult GetPageInfo() {
            var page = myDbContext.Articles.Select(article => new
            {
                Id = article.Id,
                Title = article.Title,
                num =article.ReadCount,
                datetime = article.CreatedDate,
                Content = article.Content.Length > 100 ? article.Content.Substring(0, 100) : article.Content
            }).ToList();

            return Ok( page);
        }
    }
}
