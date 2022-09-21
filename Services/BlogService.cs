using AutoMapper;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using OdataPckg.DAL;
using OdataPckg.DAL.Entities;
using OdataPckg.DTO;
using OdataPckg.Extensions;
using System.Linq.Expressions;
using System.Xml.Linq;

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

        public IEnumerable<BlogDto> Get(ODataQueryOptions<BlogDto> queryOptions)
        {
            var expression = TranslateExpression<BlogDto, Blog>(queryOptions);

            var items = blogs.Where(expression).ToList();

            return mapper.Map<IEnumerable<BlogDto>>(items);
        }

        BlogDto? IBlogService.GetById(int id)
        {
            var item = blogs.FirstOrDefault(b => b.Id == id);
            return mapper.Map<BlogDto>(item);
        }

        private Expression<Func<TEntity, bool>> TranslateExpression<TDto, TEntity>(ODataQueryOptions<TDto> queryOptions)
        {
            var filter = queryOptions.GetFilter();

            var translatedExpression = mapper.Map<Expression<Func<TDto, bool>>, Expression<Func<TEntity, bool>>>(filter);
            return translatedExpression;
        }
    }
}
