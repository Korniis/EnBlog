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
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using Tea.Utils;
namespace EBlog.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly UserDbContext _userDbContext;
        private readonly IMapper mapper;

        public ArticleController(IArticleService articleService, IMapper mapper, UserDbContext userDbContext)
        {
            this.mapper = mapper;
            _articleService = articleService;
            _userDbContext = userDbContext;
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
            var image = articleInfo.ImgSrc;
            var userId = long.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            Article article = new Article()
            {
                Title = title,
                Content = content,
                UserId = userId,
                CreateTime = DateTime.Now,
                TypeId = tid,
                ImgSrc = image,
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
        [Authorize]
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
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ApiResult>> GetArticleByUser(int pageSize, int pageNum = 1)
        {
            var id = this.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToSafeInt();

            var articles = await _userDbContext.Articles
          .Where(c => c.UserId == id)
          .OrderByDescending(c => c.CreateTime)
          .Skip((pageNum - 1) * pageSize)
          .Take(pageSize)
          .Include(c => c.User)
          .Include(c => c.Type)
          .Select(c => new ArticleDTO
          {
              Id = c.Id,
              Title = c.Title,
              CreateTime = c.CreateTime,
              UserName = c.User.UserName,
              TypeName = c.Type.TypeName,
              ImgSrc = c.ImgSrc
              // 其他你需要的字段
          })
          .ToListAsync();
            var count = await _userDbContext.Articles.CountAsync();
            if (articles == null)
            {
                return ApiResultHelper.Error("未找到文章");

            }
            List<ArticleDTO> articleDTOs = new List<ArticleDTO>();

            return ApiResultHelper.Success(new { count, articles });



        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResult>> UploadImg(IFormFile formData)
        {


            if (formData == null || formData.Length == 0)
            {
                return ApiResultHelper.Error("No file uploaded.");
                // return BadRequest(new ApiResult { Success = false, Message = "No file uploaded." });
            }
            if (formData.Length > 2 * 1024 * 1024)
            {
                return ApiResultHelper.Error("请限制在2mb");
            }


            // 获取当前用户的用户名并使用它来生成唯一文件名
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {

                return ApiResultHelper.AuthError("无用户信息");
                // return Unauthorized(new ApiResult { Success = false, Message = "User not authorized." });
            }
            Guid id = Guid.NewGuid();
            var fileExtension = Path.GetExtension(formData.FileName);
            var safeFileName = $"{id}_img{fileExtension}"; // 例如：用户名作为文件名
            var targetFilePath = Path.Combine("wwwroot/images/", safeFileName);


            try
            {

                // 保存文件到目标路径
                using (var stream = new FileStream(targetFilePath, FileMode.Create))
                {
                    await formData.CopyToAsync(stream);
                }

                return ApiResultHelper.Success(new { FileName = safeFileName, Message = "Avatar uploaded successfully." });
            }
            catch (Exception ex)
            {
                return ApiResultHelper.Error($"Error uploading avatar: {ex.Message}");
            }
        }
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ApiResult>> UpdateArticle(CreateArticleInfo articleInfo)
        {
            var article1 = await _articleService.SelectOneByIdAsync(articleInfo.tid);
            if (article1 == null)
            {
                return ApiResultHelper.Error("没有此文章");

            }
            var userId = long.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (userId != article1.UserId)
            {

                return ApiResultHelper.Error("无权修改");

            }
            var title = articleInfo.title;
            var content = articleInfo.content;

            var image = articleInfo.ImgSrc;
            article1.Content = content;
            article1.Title = title;
            if (!string.IsNullOrEmpty(image))
                article1.ImgSrc = image;





            bool result = await _articleService.UpdateAsync(article1);
            if (result)
            {
                return ApiResultHelper.Success("创建成功");
            }
            return ApiResultHelper.Error("创建失败");
        }

    }
}
