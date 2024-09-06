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
using System.Security.Cryptography;
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
        private readonly IDatabase _database;
        private readonly IDistributedCache _distributedCache;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Domain.Entities.Role> _roleManager;
        private readonly IDatabase redisdb;
        public UserController(IUserService userService, IMapper mapper, IDistributedCache distributedCache, UserManager<User> userManager, RoleManager<Domain.Entities.Role> roleManager, IDatabase redisdb, IDatabase database)
        {
            _userService = userService;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            this.redisdb = redisdb;
            _database = database;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ApiResult>> GetUserByFont()
        {
            Claim? userId = this.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return ApiResultHelper.AuthError("找不到该用户");
            }
            var userStr = await _database.StringGetAsync($"EBlog_userinfo_{userId.Value}");

            if (!userStr.IsNullOrEmpty)
            {
                return Ok(userStr.ToString());

            }
            var user = await _userManager.FindByIdAsync(userId.Value);
            if (user == null)
            {
                return ApiResultHelper.AuthError("找不到该用户");

            }
            var userDTO = _mapper.Map<UserInfoDTO>(user);
            await _database.StringSetAsync($"EBlog_userinfo_{user.Id}", JsonSerializer.Serialize(userDTO));
            await _database.KeyExpireAsync($"EBlog_userinfo_{user.Id}", TimeSpan.FromMinutes(5));
            return Ok(userDTO);
        }

        [HttpPut]
        [Authorize]

        public async Task<ActionResult<ApiResult>> UpdateUserByFont(UserInfoDTO userInfo)
        {
            Claim? ruserId = this.User.FindFirst(ClaimTypes.NameIdentifier);
            long cid = long.Parse(ruserId.Value);
            User userchange = await _userService.SelectOneByIdAsync(cid);
            if (userInfo.id != cid)
            {
                return ApiResultHelper.Error("修改失败");
            }
            if (this.User.FindFirstValue(ClaimTypes.Name) != userInfo.userName)
            {
                IdentityResult namechange = await _userManager.SetUserNameAsync(userchange, userInfo.userName);
                if (!namechange.Succeeded)
                {
                    return ApiResultHelper.Error(namechange.Errors.FirstOrDefault().Description);
                }
            }
         
            userchange.MyDescriptions = userInfo.myDescriptions;
            userchange.WxAccount = userInfo.wxAccount;
            userchange.PhoneNumber = userInfo.phoneNumber;
            IdentityResult changeResult = await _userManager.UpdateAsync(userchange);
            if (!changeResult.Succeeded)
            {
                return ApiResultHelper.Error("修改失败");
                
            }
            var userchangeDTO= _mapper.Map<User, UserInfoDTO > (userchange);
            await _database.KeyDeleteAsync($"EBlog_userinfo_{userchange.Id}");

            return Ok(userchangeDTO);
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
        {
            var email = requestInfo.emailAddress;
            var code = requestInfo.code;
            var name = requestInfo.userName;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.UserName != name)
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
      
        public async Task<ActionResult<ApiResult>> EditAvatar(IFormFile formData)
        {
            if (formData == null || formData.Length == 0)
            {
                return ApiResultHelper.Error("No file uploaded.");
               // return BadRequest(new ApiResult { Success = false, Message = "No file uploaded." });
            }
            if (formData.Length > 2 * 1024 * 1024)
            {
                return ApiResultHelper.Error("请限制在2mb");
            }


            // 获取当前用户的用户名并使用它来生成唯一文件名
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {

                return ApiResultHelper.AuthError("无用户信息");
               // return Unauthorized(new ApiResult { Success = false, Message = "User not authorized." });
            }

            var fileExtension = Path.GetExtension(formData.FileName);
            var safeFileName = $"{userId}_avatar{fileExtension}"; // 例如：用户名作为文件名
            var targetFilePath = Path.Combine("wwwroot/images", safeFileName);
            User auser = await _userManager.FindByIdAsync(userId);
            
            try
            {
                auser.Avatar = safeFileName;
                 await   _userManager.UpdateAsync(auser);
                // 保存文件到目标路径
                using (var stream = new FileStream(targetFilePath, FileMode.Create))
                {
                    await formData.CopyToAsync(stream);
                }

                return ApiResultHelper.Success(new { FileName = safeFileName, Message = "Avatar uploaded successfully." });
            }
            catch (Exception ex)
            {
                return ApiResultHelper.Error($"Error uploading avatar: {ex.Message}");
            }
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
        public async Task<ActionResult<ApiResult>> ResetPasswordByOld(ResetModel reset)
        {
            Claim? changeId = this.User.FindFirst(ClaimTypes.NameIdentifier);
            if (changeId == null)
            {
                return ApiResultHelper.Error("修改失败");
            }
            var changeuser = await _userManager.FindByIdAsync(changeId.Value);

            IdentityResult pwdresult = await _userManager.ChangePasswordAsync(changeuser, reset.oldpwd, reset. newpwd);
            if (!pwdresult.Succeeded)
            {
                return ApiResultHelper.Error(pwdresult.Errors.FirstOrDefault().Description);
            }
            changeuser.JwtVersion++;
            IdentityResult upresult = await  _userManager.UpdateAsync(changeuser);
            if (!upresult.Succeeded)
            {
                return ApiResultHelper.Error(upresult.Errors.FirstOrDefault().Description);
            }
            return ApiResultHelper.Success("修改成功请返回登陆");
        }
        [HttpPut]
        [NotCheckJwtVersion]
        public async Task<ActionResult<ApiResult>> ResetPassword(ResetPasswordModel k1)
        {
          
            var change = await _userManager.FindByNameAsync(k1.Username);
            string tokenFromRedis = await _database.StringGetAsync($"EBlog_reset_{change.UserName}");
            if (!tokenFromRedis.Equals(k1.Token))
            {
                return ApiResultHelper.Error($"验证码有误!修改密码失败!");
            }
            IdentityResult RPResult = await _userManager.ResetPasswordAsync(change, k1.Token , k1.NewPad);
            change.JwtVersion++;
            await _userManager.UpdateAsync(change);
            if (!RPResult.Succeeded)
            {
                return ApiResultHelper.Error("修改失败检查，请验证码正不正确");
            }
            return ApiResultHelper.Success("修改成功请返回登录");
        }
    }
    public record ResetPasswordModel
    {
        public string Username { get; set; }
        public string NewPad { get; set; }
        public string Token { get; set; }
    }
    public record ResetModel (string oldpwd, string newpwd);
}
