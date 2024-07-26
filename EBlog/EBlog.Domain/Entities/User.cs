using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.Domain.Entities
{
    public class User:IdentityUser<long>
    {
        public string NickName { get; set; }
        public string MyDescriptions { get; set; }
        public string WxAccount {  get; set; }


    }
}
