using EBlog.Domain.Entities;
using EBlog.IBaseService;
using EBlog.Utility;
using EBlog.WebApi.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EBlog.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly IOptionsSnapshot<JWTOptions> _settings;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public AuthorizeController(IOptionsSnapshot<JWTOptions> settings, IUserService userService, UserManager<User> userManager)
        {
            _settings = settings;
            _userService = userService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResult>> Login(CheckRequestInfo info)
        {
            User user = await _userService.SelectOneAsync(x => x.UserName == info.userName);

            if (user == null)
            {
                return ApiResultHelper.Error("用户名或密码错误");
            }
            if (await _userManager.IsLockedOutAsync(user))
            {
                return ApiResultHelper.Error($"用户{info.userName}被冻结");

            }
            bool result = await _userManager.CheckPasswordAsync(user, info.userPwd);
            if (result)
            {

                await _userManager.ResetAccessFailedCountAsync(user);
                //令牌设计
                //1.声明playlod


                //2.生成jwt

                return null;
                //3.返回jwt
                // return ApiResultHelper.Success();

            }
            else//登陆失败
            {
                //记录登录次数
                await _userManager.AccessFailedAsync(user);
                int v = await _userManager.GetAccessFailedCountAsync(user);
                return ApiResultHelper.Error($"用户名或密码错误再输入{5 - v}次锁定");

            }



        }
    }
 }
