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
    public class ArticleTypeConfig : IEntityTypeConfiguration<ArticleType>
    {
        public void Configure(EntityTypeBuilder<ArticleType> builder)
        {
            builder.ToTable("T_ArticleType");

            builder.HasQueryFilter(x => x.IsDeleted == false);

        }
    }
}
