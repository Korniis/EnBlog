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

    public  class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
    {
        protected IBaseRepository<TEntity> _respository;

        public async Task<bool> CreateAsync(TEntity entity)
        {
            return await _respository.CreateAsync(entity);
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            return await _respository.DeleteAsync(entity);
        }

        public async Task<List<TEntity>> SelectAllAsync()
        {
            return await _respository.SelectAllAsync();
        }

        public async Task<List<TEntity>> SelectAllAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _respository.SelectAllAsync(expression);
        }

        public async Task<TEntity> SelectOneAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _respository.SelectOneAsync(expression);
        }

        public async Task<TEntity> SelectOneByIdAsync(long id)
        {
            return await _respository.SelectOneByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            return await _respository.UpdateAsync(entity);
        }
    }
}
