using EBlog.Domain;
using EBlog.Domain.Entities;
using EBlog.IBaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.BaseRepository
{
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {
        private readonly UserDbContext _dbContext;
        public ArticleRepository(UserDbContext dbContext) : base(dbContext)
        {
            base._dbContext= dbContext;
            _dbContext= dbContext;
        }
    }
}
