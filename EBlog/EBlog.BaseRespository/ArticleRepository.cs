using EBlog.Domain;
using EBlog.Domain.Entities;
using EBlog.IBaseRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.BaseRepository
{
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {
        private readonly UserDbContext _dbContext;
        public ArticleRepository(UserDbContext dbContext)
        {
            base._dbContext = dbContext;
            _dbContext = dbContext;
        }

        public override async Task<List<Article>> SelectAllAsync()
        {
            return await _dbContext.Articles.Include(x => x.Type).Include(x => x.User).ToListAsync();
        }

        public override async Task<List<Article>> SelectAllAsync(Expression<Func<Article, bool>> expression)
        {
            return await _dbContext.Articles.Include(x => x.Type).Include(x => x.User).Where(expression).ToListAsync();
        }

        public override async Task<Article> SelectOneAsync(Expression<Func<Article, bool>> expression)
        {
            return await _dbContext.Articles.Include(x => x.Type).Include(x => x.User).FirstOrDefaultAsync(expression);
        }

        public override async Task<Article> SelectOneByIdAsync(long id)
        {
            return await _dbContext.Articles.Include(x => x.Type).Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<Article>> SelectAllByPageAsync(int skip, Expression<Func<Article, bool>> expression = null, Expression<Func<Article, object>> order = null)
        {
            var query = _dbContext.Articles.AsQueryable();

            // 过滤条件
            if (expression != null)
            {
                query = query.Where(expression);
            }

            // 排序
            if (order != null)
            {
                query = query.OrderByDescending(order);
            }

            // 分页
            query = query.Skip(skip * 20).Take(20)
                         .Include(x => x.Type)
                         .Include(x => x.User);

            return await query.ToListAsync();
        }
    }
}
