using EBlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.IBaseRepository
{
    public interface IArticleRepository : IBaseRepository<Article>
    {
    }
}
