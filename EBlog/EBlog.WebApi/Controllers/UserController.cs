using AutoMapper;
using EBlog.Domain.DTO;
using EBlog.Domain.Entities;
using EBlog.IBaseService;
using EBlog.Utility;
using EBlog.WebApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Tea.Utils;
namespace EBlog.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Domain.Entities.Role> _roleManager;
        private readonly IDatabase redisdb;
        public UserController(IUserService userService, IMapper mapper, IDistributedCache distributedCache, UserManager<User> userManager, RoleManager<Domain.Entities.Role> roleManager, IDatabase redisdb)
        {
            _userService = userService;
            _mapper = mapper;

            _userManager = userManager;
            _roleManager = roleManager;
            this.redisdb = redisdb;
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
                userDTOs.Add(_mapper.Map<UserDTO>(item));
            }
            return ApiResultHelper.Success(userDTOs);
        }
        [HttpPost]
        [NotCheckJwtVersion]
        public async Task<ActionResult<ApiResult>> CreateUser(CheckRequestInfo info)
        {
            User user = new User() { UserName = info.userName };
            var b = await _userManager.CreateAsync(user, info.userPwd);
            if (!b.Succeeded)
            {
                var customErrors = b.Errors.Select(e =>
                {
                    if (e.Code == "DuplicateUserName")
                    {
                        e.Description = "用户名已存在，请选择其他用户名。";
                    }
                    else if (e.Code == "PasswordTooShort")
                    {
                        e.Description = "密码太短，请选择一个至少6位字符的密码。";
                    }
                    // 添加更多自定义处理逻辑
                    return e.Description;
                });
                var errors = string.Join(", ", customErrors);
                return ApiResultHelper.Error($"创建用户失败" + errors);
            }
            //添加角色
            if (!await _userManager.IsInRoleAsync(user, "Normal"))
            {
                await _userManager.AddToRoleAsync(user, "Normal");
            }
            return ApiResultHelper.Success(b.Succeeded);
        }
        [HttpPost]
        [NotCheckJwtVersion]
        public async Task<ActionResult<ApiResult>> Register(CheckEmailInfo requestInfo)
        {   var email = requestInfo.emailAddress;
            var code = requestInfo.code;
            var name = requestInfo.userName;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null||user.UserName!=name)
            {
                return ApiResultHelper.Error("验证错误请重试");
            }

            //  var storedCode = await _distributedCache.GetAsync($"reg_{user.Email}");
            var storedCode = (await redisdb.StringGetAsync($"EBlog_reg_{user.Email}")).ToSafeString();
            // var storedCode = await redisdb.HashGetAsync($"reg_{user.Email}", "data");
            if (code != storedCode)
            {
                return ApiResultHelper.Error("验证错误或超时请重试");
            }
            IdentityResult emailConfirm = await _userManager.ConfirmEmailAsync(user, code);
            if (emailConfirm.Succeeded)
            {
                return ApiResultHelper.Success("验证成功");
            }
            return ApiResultHelper.Error("验证错误请重试");
        }
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ApiResult>> EditName(string newName)
        {
            Claim? changeId = this.User.FindFirst(ClaimTypes.NameIdentifier);
            if (changeId == null)
            {
                return ApiResultHelper.Error("修改失败");
            }
            var changeuser = await _userManager.FindByIdAsync(changeId.Value);
            if (changeuser == null)
            {
                return ApiResultHelper.Error("修改失败");
            }
            changeuser.NickName = newName;
            changeuser.JwtVersion++;
            IdentityResult changeResult = await _userManager.UpdateAsync(changeuser);
            if (!changeResult.Succeeded)
            {
                return ApiResultHelper.Error("修改失败");
            }
            return ApiResultHelper.Success(changeResult.Succeeded);
        }
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ApiResult>> ResetPassword(string newPad, string token)
        {
            Claim? ruserId = this.User.FindFirst(ClaimTypes.NameIdentifier);
            var ruser = await _userManager.FindByIdAsync(ruserId.Value);
            string tokenFromRedis = JsonSerializer.Deserialize<string>(await _distributedCache.GetAsync($"sms_{ruser.Id}"));
            if (!tokenFromRedis.Equals(token))
            {
                return ApiResultHelper.Error($"验证码有误!修改密码失败!");
            }
            IdentityResult RPResult = await _userManager.ResetPasswordAsync(ruser, token, newPad);
            ruser.JwtVersion++;
            await _userManager.UpdateAsync(ruser);
            if (!RPResult.Succeeded)
            {
                return ApiResultHelper.Error("修改失败检查，请验证码正不正确");
            }
            return ApiResultHelper.Error("修改失败");
        }
    }
}
