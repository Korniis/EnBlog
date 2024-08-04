using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.Utility
{
    public record ApiResult
    {
        public int Code { get; set; }
        public string? Message { get; set; }
        public int Total {  get; set; }
        public dynamic? Data { get; set; }
    }
}
