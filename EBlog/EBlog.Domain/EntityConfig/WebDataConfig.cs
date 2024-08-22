using EBlog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.Domain.EntityConfig
{
    public class WebDataConfig : IEntityTypeConfiguration<WebData>
    {
        public void Configure(EntityTypeBuilder<WebData> builder)
        {
            builder.ToTable("T_WebData");
        }
    }
}
