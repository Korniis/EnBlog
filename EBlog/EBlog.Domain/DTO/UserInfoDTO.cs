using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.Domain.DTO
{
    public class UserInfoDTO
    {
        public string userName { get; set; }
        public string? avatar { get; set; }
        public long id { get; set; }
        public string? nickName { get; set; }
        public string? myDescriptions { get; set; }
        public string? wxAccount { get; set; }
        public string email { get; set; }
        public string? phoneNumber { get; set; }
        
    }
}
