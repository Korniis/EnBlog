using EBlog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.Domain.EntityConfig
{
    public class ArticleConfig : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {   //配置表名
            builder.ToTable("T_Article");
            //长文本类型修改
            builder.Property(x=>x.Context).HasColumnType("Text");
            //软删除
            builder.HasQueryFilter(x => x.IsDeleted == false);
            //多映射
            builder.HasOne(x => x.Type).WithMany(x => x.Articles).HasForeignKey(x => x.TypeId);

        }
    }
}
