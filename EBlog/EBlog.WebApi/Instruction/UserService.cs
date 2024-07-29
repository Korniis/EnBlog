using EBlog.Domain;
using Microsoft.AspNetCore.Mvc;

namespace EBlog.WebApi.Instruction
{
    public class UserService : IUserService
    {
        public Task<ActionResult> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> Register()
        {
            throw new NotImplementedException();
        }
    }
}
