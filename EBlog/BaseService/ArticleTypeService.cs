using EBlog.Domain.Entities;
using EBlog.IBaseRepository;
using EBlog.IBaseService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.BaseService
{
    public class ArticleTypeService : BaseService<ArticleType>, IArticleTypeService
    {
        private  readonly IArticleTypeRepository _articleTypeRepository;

        public ArticleTypeService(IArticleTypeRepository articleTypeRepository)
        {
            base._respository = articleTypeRepository;
            _articleTypeRepository = articleTypeRepository;
        }
    }
}
