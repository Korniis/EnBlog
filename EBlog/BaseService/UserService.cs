using EBlog.Domain.Entities;
using EBlog.IBaseRepository;
using EBlog.IBaseService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.BaseService
{
    public class UserService:BaseService<User>, IUserService
    {
        private readonly IUserRespository _userService;

        public UserService(IUserRespository userService)
        {
            base._respository = userService;
            _userService = userService;
        }
    }
}
