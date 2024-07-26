using EBlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.Domain
{
    public interface IBlogSerivce
    {
        
        public  Task<List<Article>> GetAllAsync();
        public Task<Article> GetByIdAsync(int id);
        public Task<Article> GetByIdAsync(string id);

    }
}
