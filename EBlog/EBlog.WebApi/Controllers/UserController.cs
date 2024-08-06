using AutoMapper;
using EBlog.Domain.DTO;
using EBlog.Domain.Entities;
using EBlog.IBaseService;
using EBlog.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EBlog.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserController(IUserService userService, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userService = userService;
            this.mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResult>> GetUsers()
        {
            var data = await _userService.SelectAllAsync();
            if (data == null)
            {
                return ApiResultHelper.Error("找不到各用户");
            }


            List<UserDTO> userDTOs = new List<UserDTO>();
            foreach (var item in data)
            {
                userDTOs.Add(mapper.Map<UserDTO>(item));

            }
            return ApiResultHelper.Success(userDTOs);
        }
        [HttpPost]
        public async Task<ActionResult<ApiResult>> CreateUser(CheckRequestInfo info)
        {
            User user = new User() { UserName = info.userName };
            var b = await _userManager.CreateAsync(user, info.userPwd);
            if (!b.Succeeded)
            {
                return ApiResultHelper.Error($"创建用户失败");
            }

            //添加角色
            if (await _userManager.IsInRoleAsync(user, "Normal"))
            {
                await _userManager.AddToRoleAsync(user, "Normal");
            }


            return ApiResultHelper.Success(b.Succeeded);
        }
        [HttpPost]
        public async Task<ActionResult<ApiResult>> Register( )
        {
            Role role = new Role
            {
             Name="Normal"
            };
            var res= await _roleManager.CreateAsync(role);



            return Ok(res.Succeeded);
        }
    }
}
