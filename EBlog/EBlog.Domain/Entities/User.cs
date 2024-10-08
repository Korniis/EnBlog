﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.Domain.Entities
{
    public class User:IdentityUser<long>
    {
        public long? JwtVersion {  get; set; }
        public string? NickName { get; set; }
        public string? MyDescriptions { get; set; }
        public string? Avatar {  get; set; }
        public string? WxAccount {  get; set; }
        public List<Article> Articles { get; set; }=new List<Article>();

    }
}
