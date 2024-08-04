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
            builder.Services.AddSwaggerGen(c=>{
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
            builder .Services.AddCustomService();
            builder.Services.AddAutoMapper(typeof(DTOMapper));
       /*     builder.Services.AddDbContext<MyDbContext>(opt =>
            {
                string connStr = builder.Configuration.GetConnectionString("Default");
                opt.UseMySql(connStr, new MySqlServerVersion(new Version(8, 6, 20)));
            });*/
            string[] urls = new[] { "http://localhost:5173" };
            builder.Services.AddCors(options =>
                options.AddDefaultPolicy(builder => builder.WithOrigins(urls)
                .AllowAnyMethod().AllowAnyHeader().AllowCredentials()));
            builder.Services.AddDbContext<UserDbContext>(opt => {
                string connStr = builder.Configuration.GetConnectionString("Default");
                opt.UseMySql(connStr, new MySqlServerVersion(new Version(8, 6, 20)));
            });
            builder.Services.AddDataProtection();
            builder.Services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                options.SignIn.RequireConfirmedEmail = true;
            });
            var idBuilder = new IdentityBuilder(typeof(User), typeof(Role), builder.Services);
            idBuilder.AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders()
                .AddRoleManager<RoleManager<Role>>()
                .AddUserManager<UserManager<User>>();
            builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                var jwtOpt = builder.Configuration.GetSection("JWT").Get<JWTOptions>();
                byte[] keyBytes = Encoding.UTF8.GetBytes(jwtOpt.SigningKey);
                var secKey = new SymmetricSecurityKey(keyBytes);
                x.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = secKey
                };
            });


  
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        
         


    }
    public static class IOCExtend
    {
        public static IServiceCollection AddCustomService(this IServiceCollection services)
        {   //²Ö´¢²ã
             services.AddScoped<IArticleRepository, ArticleRepository>();
             services.AddScoped<IArticleTypeRepository, ArticleTypeRepository>();
            //·þÎñ²ã

            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IArticleTypeService, ArticleTypeService>();

            return services;
        }
    }
}
