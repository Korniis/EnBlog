using EBlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.Domain.DTO
{
    public class ArticleTypeDTO
    {
        public long Id { get; set; }
        public string TypeName { get; set; }

        public List<string> ArticleNames { get; set; } = new List<string>();

    }
}
