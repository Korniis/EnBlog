using AutoMapper;
using EBlog.Domain;
using EBlog.Domain.DTO;
using EBlog.Domain.Entities;
using EBlog.IBaseService;
using EBlog.Utility;
using EBlog.WebApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
namespace EBlog.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IMapper mapper;

        public ArticleController(IArticleService articleService, IMapper mapper)
        {
            this.mapper = mapper;
            _articleService = articleService;
        }


        [HttpGet]
        public async Task<ActionResult<ApiResult>> GetArticles()
        {
            List<Article> articles = await _articleService.SelectAllAsync();
            // articles.ForEach(x => x.Type = null);
            if (articles.Count == 0)
            {
                return ApiResultHelper.Error("没有更多文章");
            }
            List<ArticleDTO> articleDTOs = new List<ArticleDTO>();
            foreach (var article in articles)
            {
                articleDTOs.Add(mapper.Map<ArticleDTO>(article));
            }

            return ApiResultHelper.Success(articleDTOs);
        }
        [HttpGet("{id}")]
        [NotCheckJwtVersion]
        public async Task<ActionResult<ApiResult>> GetArticleById(int id)
        {
            Article article = await _articleService.SelectOneByIdAsync(id);
            if (article == null)
            {
                return ApiResultHelper.Error("未找到该篇文章");

            }
            ArticleDTO articleDTO = mapper.Map<ArticleDTO>(article);

            return ApiResultHelper.Success(articleDTO);



        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResult>> CreateArticle(CreateArticleInfo articleInfo)
        {
            var title = articleInfo.title;
            var content = articleInfo.content;
            var tid = articleInfo.tid;
            var userId = long.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            Article article = new Article()
            {
                Title = title,
                Content = content,
                UserId = userId,
                CreateTime = DateTime.Now,
                TypeId = tid,
                IsDeleted = false,
                ViewCount = 0,
                LikeCount = 0
            };
            bool result = await _articleService.CreateAsync(article);
            if (result)
            {
                return ApiResultHelper.Success("创建成功");
            }
            return ApiResultHelper.Error("创建失败");
        }
        [HttpDelete]
        public async Task<ActionResult<ApiResult>> Delete(long Id)
        {
            var del = await _articleService.SelectOneByIdAsync(Id);
            if (del == null)
            {
                return ApiResultHelper.Error("未找到删除目标");
            }
            del.IsDeleted = true;
            bool delb = await _articleService.DeleteAsync(del);
            if (!delb)
            {
                return ApiResultHelper.Error("删除操作失败");
            }
            return ApiResultHelper.Success("删除成功");
        }
        [HttpPut]
        public async Task<ActionResult<ApiResult>> Edit(long id, string title, string content, long tid)
        {
            var article = await _articleService.SelectOneByIdAsync(id);
            if (article == null)
            {
                return ApiResultHelper.Error("未找到修改目标");
            }
            article.Title = title;
            article.Content = content;
            article.TypeId = tid;
            var edited = await _articleService.UpdateAsync(article);
            if (!edited)
            {
                return ApiResultHelper.Error("修改失败");

            }
            return ApiResultHelper.Success("修改成功");
        }
    }
}
