using EBlog.Domain.Entities;
using EBlog.WebApi.Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;
using System.Text.Json;

namespace EBlog.WebApi.Fliters
{
    public class JwtVersionCheckFilter : IAsyncActionFilter
    {
        private readonly UserManager<User> _userManager;
        private readonly IDistributedCache _distributedCache;
        public JwtVersionCheckFilter(UserManager<User> userManager, IDistributedCache distributedCache)
        {
            _userManager = userManager;
            _distributedCache = distributedCache;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor is null)
            {
                //访问的就不是action方法
                await next();
                return;
            }
            if (descriptor.ControllerTypeInfo.GetCustomAttributes(typeof(NotCheckJwtVersionAttribute), true).Any()|| descriptor.MethodInfo.GetCustomAttributes(typeof(NotCheckJwtVersionAttribute), true).Any())
            {
                await next();    //标注了NotCheckJwtVersionAttribute就放行
                return;
            }
            //eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjUiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiMTIzNDU2IiwiSnd0VmVyc2lvbiI6IjEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJOb3JtYWwiLCJleHAiOjE3MjMyMTYyMDYsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE3MyIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE3MyJ9.a3xoL6DM5F5_IgAOaRbI8pWK352BqhZojoMx_otJnt4
            var claimJwtVersion = context.HttpContext.User.FindFirst("JwtVersion");
            if (claimJwtVersion is null)
            {
                context.Result = new ObjectResult("没有找到JwtVersion的内容") { StatusCode = 401 };
                return;
            }
            Claim? UserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (UserId is null)
            {
                context.Result = new ObjectResult("没有找到JwtVersion的内容") { StatusCode = 401 };
                return;
            }
            long jwtVersionFromClient = long.Parse(claimJwtVersion.Value);
            var data = await _distributedCache.GetStringAsync($"jwt_{UserId}");
            if (data is not null)
            {  //缓存不为空
                long jwtVersion = JsonSerializer.Deserialize<long>(data);

                if (jwtVersion > jwtVersionFromClient)
                {
                    context.Result = new ObjectResult("Jwt已过时") { StatusCode = 401 };
                    return;
                }

                await next();
                return ;
            }
            else //缓存为空
            {
                var user = await _userManager.FindByIdAsync(UserId.Value);
                if (user is null)
                {
                    context.Result = new ObjectResult("数据库中不存在该用户") { StatusCode = 401 };
                    return;
                }
             

                //找到数据
                await _distributedCache.SetStringAsync($"jwt_{UserId}", JsonSerializer.Serialize(user.JwtVersion), new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(Random.Shared.Next(7, 10)),
                    SlidingExpiration = TimeSpan.FromSeconds(5)
                });

                if (user.JwtVersion > jwtVersionFromClient)
                {
                    context.Result = new ObjectResult("Jwt已过时") { StatusCode = 401 };
                    return;
                }
            }
            await next();
        }
    }
}
