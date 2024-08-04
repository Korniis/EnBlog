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
    public class ArticleTypeRepository : BaseRepository<ArticleType>, IArticleTypeRepository
    {
        private readonly UserDbContext _dbContext;
        public ArticleTypeRepository(UserDbContext dbContext) 
        {
            base._dbContext = dbContext;
            _dbContext = dbContext;
        }

        public override async Task<List<ArticleType>> SelectAllAsync()
        {
            return  await _dbContext.ArticleTypes.Include(x=>x.Articles).ToListAsync();
        }

        public override  async Task<List<ArticleType>> SelectAllAsync(Expression<Func<ArticleType, bool>> expression)
        {
          
            return  await _dbContext.ArticleTypes.Include(x=>x.Articles).Where(expression).ToListAsync();
        }

        public override async Task<ArticleType> SelectOneAsync(Expression<Func<ArticleType, bool>> expression)
        {
           
            return  await _dbContext.ArticleTypes.Include(x=>x.Articles).SingleOrDefaultAsync(expression);
        }
         
        public override async Task<ArticleType> SelectOneByIdAsync(long id)
        {
            return await _dbContext.ArticleTypes.Include(x => x.Articles).SingleOrDefaultAsync(x=>x.Id==id);

        }
    }
}
