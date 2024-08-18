using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.Utility
{
    public record CheckRequestInfo(string userName, string userPwd);
    public record CheckEmailInfo(string userName, string userPwd, string emailAddress, string? code);
    public record CreateArticleInfo(string title, string content, long tid);

}
