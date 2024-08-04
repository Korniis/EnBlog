﻿using EBlog.Domain;
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
            _dbContext= dbContext;
        }

        public override async Task<List<Article>> SelectAllAsync()
        {
            return await _dbContext.Articles.Include(x=>x.Type).ToListAsync();
        }

        public override async Task<List<Article>> SelectAllAsync(Expression<Func<Article, bool>> expression)
        {
            return  await _dbContext.Articles.Include(x => x.Type).Where(expression).ToListAsync();
        }

        public override async Task<Article> SelectOneAsync(Expression<Func<Article, bool>> expression)
        {
            return await  _dbContext.Articles.Include(x=>x.Type).FirstOrDefaultAsync(expression);
        }

        public override async Task<Article> SelectOneByIdAsync(long id)
        {
            return await _dbContext.Articles.Include(x => x.Type).FirstOrDefaultAsync(x=>x.Id==id);
        }
    }
}
