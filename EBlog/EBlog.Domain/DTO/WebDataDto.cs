using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.Domain.DTO
{
    public class WebDataDto
    {
        public long ViewCount { get; set; }
        public long ArticleCount { get; set; }
        public long TypeCount { get; set; }
        public long UserCount { get; set; }
    }
}
