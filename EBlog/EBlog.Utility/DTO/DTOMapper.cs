using AutoMapper;
using EBlog.Domain.DTO;
using EBlog.Domain.Entities;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.Utility.DTO
{
    public class DTOMapper : Profile
    {
        public DTOMapper()
        {
             base.CreateMap<Article,ArticleDTO>().ForMember(x => x.TypeName, opt =>
             {
                 opt.MapFrom(src => src.Type.TypeName);
             }).ForMember(x => x.UserName, opt =>
             {
                 opt.MapFrom(src => src.User.UserName);

             });

            base.CreateMap<ArticleType, ArticleTypeDTO>().ForMember(x => x.ArticleNames, opt =>
            {
                opt.MapFrom(src => src.Articles.Select(x=>x.Title));
            });
            base.CreateMap<User, UserDTO>().ForMember(x => x.ArticleNames, opt =>
            {
                opt.MapFrom(src => src.Articles.Select(x => x.Title));
            });
            base.CreateMap<User, UserInfoDTO>();
        
        }
    }
}
