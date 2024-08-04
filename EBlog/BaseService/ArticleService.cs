using EBlog.Domain.Entities;
using EBlog.IBaseRepository;
using EBlog.IBaseService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.BaseService
{
    public class ArticleService : BaseService<Article>, IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        public ArticleService(IArticleRepository articleRepository)
        {
            base._respository = articleRepository;
            _articleRepository = articleRepository;

        }
    }
}
