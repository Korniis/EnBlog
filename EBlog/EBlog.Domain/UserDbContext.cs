using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBlog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EBlog.Domain
{
    public class UserDbContext : IdentityDbContext<User, Role, long>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
