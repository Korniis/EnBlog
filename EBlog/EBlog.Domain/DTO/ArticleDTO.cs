using EBlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.Domain.DTO
{
    public class ArticleDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreateTime { get; set; }

        public string TypeName { get; set; }
        public string ImgSrc { get; set; }

        //指定一个与数据库同名同类型的UserId作外键: 需要在Config再配置
        public string UserName { get; set; }

        public int ViewCount { get; set; }

        public int LikeCount { get; set; }

        public bool IsDeleted { get; set; }


    }
}
