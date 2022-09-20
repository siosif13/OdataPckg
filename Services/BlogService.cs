using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OdataPckg.DAL;
using OdataPckg.DAL.Entities;
using OdataPckg.DTO;

namespace OdataPckg.Services
{
    public class BlogService : IBlogService
    {
        private readonly BloggingContext context;
        private readonly IMapper mapper;
        private readonly DbSet<Blog> blogs;

        public BlogService(BloggingContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
            blogs = context.Set<Blog>();
        }

        IEnumerable<BlogDto> IBlogService.Get()
        {
            var items = blogs.ToList();
            return mapper.Map<IEnumerable<BlogDto>>(items);
        }

        BlogDto? IBlogService.GetById(int id)
        {
            var item = blogs.FirstOrDefault(b => b.Id == id);
            return mapper.Map<BlogDto>(item);
        }
    }
}
