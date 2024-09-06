using EBlog.Domain;
using EBlog.Domain.Entities;
using EBlog.IBaseService;
using EBlog.Utility;
using EBlog.WebApi.Attributes;
using EBlog.WebApi.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using StackExchange.Redis;

using System.Text.Json;
namespace EBlog.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly IOptionsSnapshot<JWTOptions> _settings;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly IDistributedCache _distributedCache;
        private readonly IDatabase _redisDatabase;

        public AuthorizeController(IOptionsSnapshot<JWTOptions> settings, IDistributedCache distributedCache, IUserService userService, UserManager<User> userManager, UserDbContext userDbContext, IDatabase redisDatabase)
        {
            _settings = settings;
            _userService = userService;
            _userManager = userManager;
            _distributedCache = distributedCache;
            _redisDatabase = redisDatabase;
        }
        [HttpPost]
        [NotCheckJwtVersion]
        public async Task<ActionResult<ApiResult>> Login(CheckRequestInfo info)
        {
            // await   _distributedCache.GetAsync("")
            User user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == info.userName);
            if (user == null)
            {
                return ApiResultHelper.Error("用户名或密码错误");
            }
            if (await _userManager.IsLockedOutAsync(user))
            {
                return ApiResultHelper.Error($"用户{info.userName}被冻结，请稍后重试");
            }
            bool result = await _userManager.CheckPasswordAsync(user, info.userPwd);
            if (result)
            {
                await _userManager.ResetAccessFailedCountAsync(user);
                user.JwtVersion++;
                await _userManager.UpdateAsync(user);
                //令牌设计
                //1.声明payload
                List<Claim> claims = new List<Claim>()
                {
                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                 new Claim (ClaimTypes.Name,user.UserName),
                  new ("JwtVersion",user.JwtVersion.ToString())
                };
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                //2.生成jwt
                string key = _settings.Value.SigningKey;
                DateTime dateTime = DateTime.Now.AddSeconds(_settings.Value.ExpireSeconds);
                byte[] sceBytes = Encoding.UTF8.GetBytes(key);
                SymmetricSecurityKey sceKey = new SymmetricSecurityKey(sceBytes);
                SigningCredentials credentials = new SigningCredentials(sceKey, SecurityAlgorithms.HmacSha256Signature);
                JwtSecurityToken securityToken = new JwtSecurityToken(claims: claims,
                    expires: dateTime,
                    issuer: _settings.Value.Issuer,
                    audience: _settings.Value.Audience,
                    signingCredentials: credentials
                    );
                string jwt = new JwtSecurityTokenHandler().WriteToken(securityToken);
                //3.返回jwt
                return ApiResultHelper.Success(jwt);
            }
            else//登陆失败
            {
                //记录登录次数
                await _userManager.AccessFailedAsync(user);
                int v = await _userManager.GetAccessFailedCountAsync(user);
                return ApiResultHelper.Error($"用户名或密码错误再输入{5 -v}次锁定");
            }
        }
        [HttpPost]
        [NotCheckJwtVersion]
        public async Task<ActionResult<ApiResult>> SendRegisterCode(CheckEmailInfo requestInfo)
        {
            var userName = requestInfo.userName;
            var password = requestInfo.userPwd;
            var emailAddress = requestInfo.emailAddress;
            //  var user  await   _userManager.Users(x => x.UserName == userName);
            var user = await _userManager.FindByEmailAsync(emailAddress);
            IdentityResult b;
            if (user is not null)
            {
                if (user.EmailConfirmed)
                    return ApiResultHelper.Error("该邮箱已被创建请返回登录");
                user.Email = emailAddress;
                user.UserName = userName;
                user.JwtVersion = 0;
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, password);
                b = await _userManager.UpdateAsync(user);
                if (!b.Succeeded)
                {
                    return ApiResultHelper.Error(b.Errors.First().Description);
                }
            }
            else
            {
                user = new User()
                {
                    Email = emailAddress,
                    UserName = userName,
                    JwtVersion = 0
                };
                b = await _userManager.CreateAsync(user, password);
            }
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
            await _userManager.AddToRoleAsync(user, "Normal");
            string confirmcode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var isSend = true;//await SMTPHelper.UseSmtpAsync(user.Email, "注册码", confirmcode);

            await _redisDatabase.StringSetAsync($"EBlog_reg_{user.Email}", confirmcode, TimeSpan.FromMinutes(3)); 
            if (isSend)
            {
                return ApiResultHelper.Success("创建成功");
            }
            else
            {
                return ApiResultHelper.Error("请重试");
            }
        }
        [HttpGet]
        [NotCheckJwtVersion]
        public async Task<ActionResult<ApiResult>> SendResetToken(string username ,string email )
        {
            var change =await _userManager.FindByNameAsync(username);
            if (change == null)
            {
                return ApiResultHelper.Error("请注册");
            }
            if(change.Email!=email)
            {

                return ApiResultHelper.Error("邮箱错误");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(change);
            await  _redisDatabase.StringSetAsync($"EBlog_reset_{change.UserName}", token,TimeSpan.FromMinutes(5));
            // 发送信息
            //SMTPHelper.UseSmtpAsync(change.Email, "code",token);
            return ApiResultHelper.Success("请查看邮箱");

            /*  Claim? UserId = this.User.FindFirst(ClaimTypes.NameIdentifier);
              if (UserId == null)
              {
                  return ApiResultHelper.Error("请登录");
              }*/

            /*    var Ruser = await _userManager.FindByIdAsync(UserId.Value);
                if (Ruser == null)
                {
                    return ApiResultHelper.Error("发送失败");
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(Ruser);

                // 发送信息
                // SMSHelper.UseSMS("18353146519", token);
                await _distributedCache.SetStringAsync($"sms_{Ruser.Id}", JsonSerializer.Serialize(token), new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(5)
                });
                return ApiResultHelper.Success(token);*/
        }
    }
}
