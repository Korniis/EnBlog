using AutoMapper;
using EBlog.Domain;
using EBlog.Domain.DTO;
using EBlog.Domain.Entities;
using EBlog.IBaseService;
using EBlog.Utility;
using EBlog.WebApi.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using Tea.Utils;

namespace EBlog.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [NotCheckJwtVersion]

    public class MainViewController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IDatabase _redisDatabase;
        private readonly IMapper mapper;
        private readonly UserDbContext _userDbContext;

        public MainViewController(IArticleService articleService, IDatabase redisDatabase, IMapper mapper, UserDbContext userDbContext)
        {
            _articleService = articleService;
            _redisDatabase = redisDatabase;
            this.mapper = mapper;
            _userDbContext = userDbContext;
        }

        [HttpGet]
        [NotCheckJwtVersion]
        public async Task<ApiResult> UpWebData()
        {
            var viewdata = await _redisDatabase.HashGetAllAsync("Eblog_ViewData");
            if (viewdata.IsNullOrEmpty())
            {

                var dto = new WebDataDto
                {
                    ViewCount = 1,
                    ArticleCount = await _userDbContext.Articles.CountAsync(),
                    TypeCount = await _userDbContext.ArticleTypes.CountAsync(),
                    UserCount = await _userDbContext.Users.CountAsync()
                };

                var hashEntries = new HashEntry[]
                {
                      new HashEntry("ViewCount", dto.ViewCount),
                      new HashEntry("ArticleCount", dto.ArticleCount),
                      new HashEntry("TypeCount", dto.TypeCount),
                      new HashEntry("UserCount", dto.UserCount)
                };
                await _redisDatabase.HashSetAsync("Eblog_ViewData", hashEntries);


                return ApiResultHelper.Success(dto);
            }

            else
            {
                await _redisDatabase.HashIncrementAsync("Eblog_ViewData", "ViewCount");
                var dto = new WebDataDto
                {
                    ViewCount = (long)viewdata.FirstOrDefault(entry => entry.Name == "ViewCount").Value + 1,
                    ArticleCount = (long)viewdata.FirstOrDefault(entry => entry.Name == "ArticleCount").Value,
                    TypeCount = (long)viewdata.FirstOrDefault(entry => entry.Name == "TypeCount").Value,
                    UserCount = (long)viewdata.FirstOrDefault(entry => entry.Name == "UserCount").Value
                };

                return ApiResultHelper.Success(dto);

            }


        }
        [HttpGet("{PageNum}")]
        public async Task<ActionResult<ApiResult>> GetArticle(int PageNum)
        {
            List<Article> articles = await _articleService.SelectAllByPageAsync(PageNum, null, c => c.CreateTime);
            List<ArticleDTO> articleDTO = new List<ArticleDTO>();
            foreach (var art in articles)
            {
                articleDTO.Add(mapper.Map<ArticleDTO>(art));
            }
            return ApiResultHelper.Success(articleDTO);
        }

    }
}
