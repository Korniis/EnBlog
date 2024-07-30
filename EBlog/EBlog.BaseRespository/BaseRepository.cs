using EBlog.Domain;
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
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        protected UserDbContext _dbContext;
        protected BaseRepository(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAsync(TEntity entity)
        {
            return await UpdateAsync(entity);
        }
        public async Task<List<TEntity>> SelectAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }
        public async Task<List<TEntity>> SelectAllAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbContext.Set<TEntity>().Where(expression).ToListAsync();
        }
        public async Task<TEntity> SelectOneAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(expression);
        }
        public async Task<TEntity> SelectOneByIdAsync(long id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

    }
}
