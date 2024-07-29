using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EBlog.Domain
{
    public interface IUserService
    {
        public Task<ActionResult> Login(string username, string password);
        public Task<ActionResult> Register();


    }
}
