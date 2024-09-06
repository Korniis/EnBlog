using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.Domain.Entities
{
    public class Article:BaseId
    {

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreateTime { get; set; }

        public long TypeId { get; set; }
        public string? ImgSrc {  get; set; } 

        //指定一个与数据库同名同类型的UserId作外键: 需要在Config再配置
        public long? UserId { get; set; }

        public int ViewCount { get; set; }

        public int LikeCount { get; set; }

        public bool IsDeleted { get; set; }

        public User? User { get; set; }
        public ArticleType Type { get; set; }

    }
}
