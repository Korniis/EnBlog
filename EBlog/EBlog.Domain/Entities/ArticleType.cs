using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.Domain.Entities
{
    public class ArticleType:BaseId
    {

        public string TypeName { get; set; }

        public List<Article> Articles { get; set; } = new List<Article>();

        public bool IsDeleted { get; set; } 
    }
}
