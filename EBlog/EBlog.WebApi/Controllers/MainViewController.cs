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

namespace EBlog.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [NotCheckJwtVersion]

    public class MainViewController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IMapper mapper;
        private readonly UserDbContext  _userDbContext;

        public MainViewController(IArticleService articleService, IMapper mapper, UserDbContext userDbContext)
        {
            _articleService = articleService;
            this.mapper = mapper;
            _userDbContext = userDbContext;
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
