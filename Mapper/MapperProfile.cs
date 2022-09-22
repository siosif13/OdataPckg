using AutoMapper;
using OdataPckg.DAL.Entities;
using OdataPckg.DTO;

namespace OdataPckg.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Blog, BlogDto>().ReverseMap();

            CreateMap<Post, PostDto>().ReverseMap();
        }
    }
}
