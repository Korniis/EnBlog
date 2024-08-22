using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.Domain.Entities
{
    public class WebData:BaseId
    {
        public long ViewCount { get; set; }
        
        public long DayAdd { get; set; }
        public long UserLogin { get; set; }
        
        public DateTime toDay {  get; set; } 


    }
}
