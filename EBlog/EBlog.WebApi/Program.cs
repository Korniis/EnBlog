using Microsoft.EntityFrameworkCore;
using EBlog.Domain;
using EBlog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using EBlog.WebApi.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using EBlog.IBaseRepository;
using EBlog.BaseRepository;
using EBlog.IBaseService;
using EBlog.BaseService;
using EBlog.Utility.DTO;
using Microsoft.AspNetCore.Mvc;
using EBlog.WebApi.Fliters;
namespace EBlog.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                var scheme = new OpenApiSecurityScheme()
                {
                    Description = "Authorization header. \r\nExample: 'Bearer 12345abcdef'",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Authorization"
                    },
                    Scheme = "oauth2",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                };
                c.AddSecurityDefinition("Authorization", scheme);
                var requirement = new OpenApiSecurityRequirement();
                requirement[scheme] = new List<string>();
                c.AddSecurityRequirement(requirement);
            });
            builder.Services.AddDbContext<UserDbContext>(opt =>
            {
                string connStr = builder.Configuration.GetConnectionString("Default");
                opt.UseMySql(connStr, new MySqlServerVersion(new Version(8, 6, 20)));
            });
            builder.Services.AddCustomIOC();
            builder.Services.Configure<MvcOptions>(opt => {

                opt.Filters.Add<JwtVersionCheckFilter>();
                opt.Filters.Add<EmailCheckFilter>();
            });
            builder.Services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = "127.0.0.1";
                opt.InstanceName = "blog_";
            });
            builder.Services.AddAutoMapper(typeof(DTOMapper));
            builder.Services.AddIdentityIOC(builder.Configuration);
            string[] urls = new[] { "http://localhost:5173" };
            builder.Services.AddCors(options =>
                options.AddDefaultPolicy(builder => builder.WithOrigins(urls)
                .AllowAnyMethod().AllowAnyHeader().AllowCredentials()));
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthentication(); //��Ȩ
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
    public static class IOCExtend
    {
        public static IServiceCollection AddCustomIOC(this IServiceCollection services)
        {   //�ִ���
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IArticleTypeRepository, ArticleTypeRepository>();
            services.AddScoped<IUserRespository, UserRespostiory>();
            //�����
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IArticleTypeService, ArticleTypeService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
        public static IServiceCollection AddIdentityIOC(this IServiceCollection services, IConfiguration Configuration)
        {   //���ݱ���
            services.AddDataProtection();
            //���Ŀ��
            services.AddIdentityCore<User>(options =>
            {   //����
                options.Password.RequireDigit = true; //����������
                options.Password.RequireLowercase = false; //Сд
                options.Password.RequireNonAlphanumeric = false; //����
                options.Password.RequireUppercase = false; //��д
                options.Password.RequiredLength = 6; //��̳���
                //����
                options.Lockout.MaxFailedAccessAttempts = 5;//����¼����
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);//�����¼�
                //��֤
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider; //����������֤
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider; //ע��������֤
            });

            var idBuilder = new IdentityBuilder(typeof(User), typeof(Role), services);
            idBuilder.AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders()
                .AddRoleManager<RoleManager<Role>>()
                .AddUserManager<UserManager<User>>();
             services.Configure<JWTOptions>(Configuration.GetSection("JWT"));
            var jwtOpt = Configuration.GetSection("JWT");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                var jwtOpt = Configuration.GetSection("JWT").Get<JWTOptions>();
                byte[] keyBytes = Encoding.UTF8.GetBytes(jwtOpt.SigningKey);
                var secKey = new SymmetricSecurityKey(keyBytes);
                x.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOpt.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOpt.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = secKey,
                    ClockSkew = TimeSpan.FromSeconds(jwtOpt.ExpireSeconds)


                };
            });
            return services;
        }
    }
     
}
