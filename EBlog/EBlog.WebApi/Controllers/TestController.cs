﻿using EBlog.Domain;
using EBlog.Domain.Entities;
using EBlog.WebApi.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Text;
namespace EBlog.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly UserDbContext UserDbContext;
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;
        private static string BuildToken(IEnumerable<Claim> claims, JWTOptions options)
        {
            DateTime expires = DateTime.Now.AddSeconds(options.ExpireSeconds);
            byte[] keyBytes = Encoding.UTF8.GetBytes(options.SigningKey);
            var secKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(secKey,
                SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(expires: expires,
                signingCredentials: credentials, claims: claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        public TestController(UserDbContext UserDbContext, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.UserDbContext = UserDbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        [HttpGet]
        public ActionResult GetPageInfo()
        {
            var page = UserDbContext.Articles.Select(article => new
            {
                Id = article.Id,
                Title = article.Title,
                num = article.ViewCount,
                datetime = article.CreateTime,
                Content = article.Context.Length > 100 ? article.Context.Substring(0, 100) : article.Context
            }).ToList();
            return Ok(page);
        }
        [HttpPost]
        public async Task<ActionResult> AddMan()
        {
            bool roleExists = await roleManager.RoleExistsAsync("admin");
            if (!roleExists)
            {
                Role role = new Role { Name = "Admin" };
                var r = await roleManager.CreateAsync(role);
                if (!r.Succeeded)
                {
                    return BadRequest(r.Errors);
                }
            }
            User user = await this.userManager.FindByNameAsync("yzk");
            if (user == null)
            {
                user = new User { UserName = "yzk", Email = "hello", EmailConfirmed = true };
                var r = await userManager.CreateAsync(user, "123456");
                if (!r.Succeeded)
                {
                    return BadRequest(r.Errors);
                }
                r = await userManager.AddToRoleAsync(user, "admin");
                if (!r.Succeeded)
                {
                    return BadRequest(r.Errors);
                }
            }
            return Ok();
        }
        [HttpGet]
        public async Task<ActionResult> GetResult()
        {
            var users = userManager.Users.ToList();
            return Ok(users);
        }
        [HttpPost]
        public async Task<ActionResult> AddRole()
        {
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult> AddUser(string account, string password)
        {
            //  User rguser = new User { UserName = account, Email = account };
            // IdentityResult identityResult = await userManager.CreateAsync(rguser, password);
            User rguser=await userManager.FindByEmailAsync(account);
            string token = await userManager.GenerateEmailConfirmationTokenAsync(rguser);
            return Ok(token);
        }
        [HttpPost]
        public async Task<ActionResult> confimem(string account, string code)
        {
            User task = await userManager.FindByEmailAsync(account);


            IdentityResult identityResult = await userManager.ConfirmEmailAsync(task, code);
        
            return Ok(identityResult);
        }
        [HttpPost]
        public async Task<ActionResult> SendEmild(string em)
        {
            if (string.IsNullOrEmpty(em))
            {
                return BadRequest();
            }
            User rguser = new User { UserName = em, Email = em  };
            string CfCode = await userManager.GenerateEmailConfirmationTokenAsync(rguser);
            return Ok(CfCode);
        }
    }
}
