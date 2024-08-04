using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.IBaseService
{
    public interface IBaseService<TEntity> where TEntity : class, new()
    {
        //增删改查
        /// <summary>
        /// 创建数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<bool> CreateAsync(TEntity entity);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(TEntity entity);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<bool> UpdateAsync(TEntity entity);
        /// <summary>
        /// 查找所有
        /// </summary>
        /// <returns></returns>
        public Task<List<TEntity>> SelectAllAsync();
        /// <summary>
        /// 根据id查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<TEntity> SelectOneByIdAsync(long id);
        /// <summary>
        /// 自定义查找单个
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Task<TEntity> SelectOneAsync(Expression<Func<TEntity, bool>> expression);
        /// <summary>
        /// 自定义查找多个
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Task<List<TEntity>> SelectAllAsync(Expression<Func<TEntity, bool>> expression);




    }
}
