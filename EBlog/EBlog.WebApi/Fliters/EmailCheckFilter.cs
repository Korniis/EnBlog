using EBlog.Domain.Entities;
using EBlog.WebApi.Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;

namespace EBlog.WebApi.Fliters
{
    public class EmailCheckFilter : IAsyncActionFilter
    {
        private readonly UserManager<User> _userManager;
        private readonly IDistributedCache _distributedCache;
        public EmailCheckFilter(UserManager<User> userManager, IDistributedCache distributedCache)
        {
            _userManager = userManager;
            _distributedCache = distributedCache;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {   //eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjUiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiMTIzNDU2IiwiSnd0VmVyc2lvbiI6IjkiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJOb3JtYWwiLCJleHAiOjE3MjMzOTg1NjQsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE3MyIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE3MyJ9.3oeAtUPKRB2aqJ1W422FGhxFGf6GmgUshvz6Zd5cJ70
            var desc = context.ActionDescriptor as ControllerActionDescriptor;
            if (desc is null)
            {
                await next();
                return;
            }

            if (desc.MethodInfo.GetCustomAttributes(typeof(CheckEmailAttribute), true).Any())
            {
                var user = await _userManager.FindByIdAsync(context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (user == null || !user.EmailConfirmed)
                {
                    context.Result = new ObjectResult("请验证邮箱") { StatusCode = 401 };
                    return;
                }
                await next();
                return;
            }
           
            await next();
            
        }
    }
}
