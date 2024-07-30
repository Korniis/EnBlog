using EBlog.Domain;
using EBlog.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EBlog.WebApi.Instruction
{
    public class BlogService : IBlogSerivce
    {
        public readonly UserDbContext ctx;
        public readonly UserManager<User> userManager;

        public BlogService(UserDbContext ctx, UserManager<User> userManager)
        {
            this.ctx = ctx;
            this.userManager = userManager;
        }

        public async Task<List<Article>> GetAllAsync()
        {
            return ctx.Articles.OrderByDescending(c => c.CreateTime).Take(12).ToList();
        }

        public async Task<Article> GetByIdAsync(int id)
        {
            return ctx.Articles.FirstOrDefault(a => a.Id == id);
        }

        public async Task<Article> GetByIdAsync(string name)
        {
            return ctx.Articles.FirstOrDefault(a => a.Context == name);

        }



    }
}
