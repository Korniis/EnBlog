using EBlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.IBaseService
{
    public interface IArticleService : IBaseService<Article>
    {
        public Task<List<Article>> SelectAllByPageAsync(int skip, Expression<Func<Article, bool>> expression = null, Expression<Func<Article, object>> order = null);

    }
}
