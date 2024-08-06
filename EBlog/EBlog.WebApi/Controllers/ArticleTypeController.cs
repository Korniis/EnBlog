using AutoMapper;
using EBlog.Domain.DTO;
using EBlog.Domain.Entities;
using EBlog.IBaseService;
using EBlog.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EBlog.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticleTypeController : ControllerBase
    {
        private readonly IArticleTypeService _ArticleTypeService;
        private readonly IMapper mapper;

        public ArticleTypeController(IArticleTypeService articleTypeService, IMapper mapper)
        {
            _ArticleTypeService = articleTypeService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResult>> GetArticleTypes()
        {
            List<ArticleType> articleTypes = await _ArticleTypeService.SelectAllAsync();
            if (articleTypes.Count == 0)
            {
                return ApiResultHelper.Error("没有更多文章类型");
            }
            List<ArticleTypeDTO> typeDTOs = new List<ArticleTypeDTO>();
            foreach (var a in articleTypes)
            {
                typeDTOs.Add(mapper.Map<ArticleTypeDTO>(a));
            }
            return ApiResultHelper.Success(typeDTOs);
        }
        [HttpPost]
        public async Task<ActionResult<ApiResult>> CreateArticleType( string typeName)
        {
            ArticleType articleType = new ArticleType()
            {
            TypeName = typeName,
            IsDeleted=false,
            };
            bool result = await _ArticleTypeService.CreateAsync(articleType);
            if (result)
            {
                return ApiResultHelper.Success(articleType);
            }
            return ApiResultHelper.Error($"{articleType}创建失败");
        }
        [HttpDelete]
        public async Task<ActionResult<ApiResult>> Delete(long Id)
        {
            var del = await _ArticleTypeService.SelectOneByIdAsync(Id);
            if (del is null)
            {
                return ApiResultHelper.Error("未找到删除目标");
            }
            del.IsDeleted = true;
            bool delb = await _ArticleTypeService.DeleteAsync(del);
            if (!delb)
            {
                return ApiResultHelper.Error("删除操作失败");
            }
            return ApiResultHelper.Success("删除成功");
        }
        [HttpPut]
        public async Task<ActionResult<ApiResult>> Edit(long id,string typeName)
        {
            var ArticleType = await _ArticleTypeService.SelectOneByIdAsync(id);
            if (ArticleType == null)
            {
                return ApiResultHelper.Error("未找到修改目标");
            }
       
            ArticleType.TypeName = typeName;
            var edited = await _ArticleTypeService.UpdateAsync(ArticleType);
            if (!edited)
            {
                return ApiResultHelper.Error("修改失败");

            }
            return ApiResultHelper.Success("修改成功");
        }

        public override bool Equals(object? obj)
        {
            return obj is ArticleTypeController controller &&
                   EqualityComparer<IMapper>.Default.Equals(mapper, controller.mapper);
        }
    }
}
