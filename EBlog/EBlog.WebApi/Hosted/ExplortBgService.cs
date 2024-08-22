
using EBlog.Domain;
using EBlog.Domain.Entities;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace EBlog.WebApi.Hosted
{

     


    public class ExplortBgService : BackgroundService
    { private readonly UserDbContext _userDbContext;
        private readonly IDatabase _redisDatabase;
        private readonly TimeSpan _executionTime = TimeSpan.FromHours(0);
        private readonly IServiceScope serviceScope;
        public ExplortBgService(IServiceScopeFactory scopeFactory)
        {
            this.serviceScope = scopeFactory.CreateScope();
            var sp = serviceScope.ServiceProvider;
            _userDbContext = sp.GetRequiredService<UserDbContext>();
            _redisDatabase= sp.GetRequiredService<IDatabase>();

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                var currentTime = DateTime.UtcNow;
                var nextRunTime = currentTime.Date.AddDays(1);
                var delay = nextRunTime - currentTime;
                await Task.Delay(delay, stoppingToken);

                if (stoppingToken.IsCancellationRequested)
                    break;
                WebData webData = new WebData()
                {

                    ViewCount = (long)await _redisDatabase.HashGetAsync("Eblog_ViewData", "ViewCount"),
                    toDay = DateTime.UtcNow,
                    UserLogin = await _userDbContext.Users.CountAsync(),
                    DayAdd = await _userDbContext.Articles.CountAsync(),
                    

                };
                try
                {
                    await _userDbContext.AddAsync(webData);
                    await _userDbContext.SaveChangesAsync();
                    await _redisDatabase.KeyDeleteAsync("Eblog_ViewData");

                }
                catch (Exception ex)
                {
                    Console.WriteLine("托管出错");
                }
            }

        }
    }
}
