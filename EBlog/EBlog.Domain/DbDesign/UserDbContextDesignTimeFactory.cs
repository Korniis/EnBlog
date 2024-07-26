using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.Domain.DbDesign
{
    public class UserDbContextDesignTimeFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<UserDbContext> builder = new DbContextOptionsBuilder<UserDbContext>();


            string jsonfile = "DbSettings.json ";
            string connStr = "";
            using (StreamReader file = File.OpenText(jsonfile))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(reader);
                    connStr = o["connStr"].ToString();
                }
            }
            builder.UseMySql(connStr, new MySqlServerVersion(new Version(8, 6, 20)));

            return new UserDbContext(builder.Options);

        }
    }
}
