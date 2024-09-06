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
using StackExchange.Redis;
using Microsoft.Extensions.Options;
using EBlog.WebApi.Hosted;
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
            builder.Services.AddCustomRedis(builder.Configuration);
            builder.Services.AddCustomIOC();
            builder.Services.Configure<MvcOptions>(opt => {

                opt.Filters.Add<JwtVersionCheckFilter>();
                opt.Filters.Add<EmailCheckFilter>();
            });
           
            builder.Services.AddAutoMapper(typeof(DTOMapper));
            builder.Services.AddIdentityIOC(builder.Configuration);
            string[] urls = new[] { "http://localhost:5173" };
            builder.Services.AddCors(options =>
                options.AddDefaultPolicy(builder => builder.WithOrigins(urls)
                .AllowAnyMethod().AllowAnyHeader().AllowCredentials()));
            builder.Services.AddHostedService<ExplortBgService>();

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
            app.UseStaticFiles();
            app.MapControllers();
            app.Run();
        }
    }
    public static class IOCExtend
    {
        public static IServiceCollection AddCustomRedis( this IServiceCollection services ,IConfiguration Configuration)
        {
            var redisConfiguration = Configuration.GetSection("Redis:ConnectionString").Value;
            var instanceName = Configuration.GetSection("Redis:InstanceName").Value;

            // ���� StackExchange.Redis ����
            services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = redisConfiguration;
                opt.InstanceName = instanceName;
            });

            // ���� IConnectionMultiplexer
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var options = ConfigurationOptions.Parse(redisConfiguration);
                // ������Ҫ������������ѡ��
                // options.Password = "your_redis_password"; // �����Ҫ����
                return ConnectionMultiplexer.Connect(options);
            });

            // ���� IDatabase
            services.AddTransient<IDatabase>(sp =>
            {
                var connectionMultiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
                return connectionMultiplexer.GetDatabase();
            });

            return services;
        }
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
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);//�����¼�
                //��֤
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider; //����������֤
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider; //ע��������֤
            });

            var idBuilder = new IdentityBuilder(typeof(User), typeof(Domain.Entities.Role), services);
            idBuilder.AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders()
                .AddRoleManager<RoleManager<Domain.Entities.Role>>()
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
