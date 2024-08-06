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
    public class UserRespostiory:BaseRepository<User>,IUserRespository
    {
        private readonly UserDbContext _dbContext;

        public UserRespostiory(UserDbContext userDbContext)
        {
            base._dbContext = userDbContext;
            this._dbContext = userDbContext;
        }

        public async override Task<List<User>> SelectAllAsync()
        {
            return await _dbContext.Users.Include(x=>x.Articles).ToListAsync();
        }

        public  async override Task<List<User>> SelectAllAsync(Expression<Func<User, bool>> expression)
        {
            return await _dbContext.Users.Where(expression).Include(x=>x.Articles).ToListAsync();
        }

        public async override Task<User> SelectOneAsync(Expression<Func<User, bool>> expression)
        {
            return  await _dbContext.Users.Include(x => x.Articles).SingleOrDefaultAsync(expression);
        }

        public override async Task<User> SelectOneByIdAsync(long id)
        {
            return await _dbContext.Users.Include(x => x.Articles).SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
