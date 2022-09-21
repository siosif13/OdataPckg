using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using AutoMapper.QueryableExtensions;
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
            var initialQ = context.Set<Blog>().Include(p => p.Posts);
            // Take & Skip are not called in the database

            var query = TranslateQuery(initialQ, queryOptions);

            var items = mapper.Map<IEnumerable<BlogDto>>(query.ToList());
            //queryOptions.ApplyTo(items);

            return items;
        }


        public IEnumerable<BlogDto> GetTakeSkip(ODataQueryOptions<BlogDto> queryOptions)
        {
            //var expression = TranslateExpression<BlogDto, Blog>(queryOptions);

            //var items = blogs.Where(expression).ToList();

            //var items = blogs.ToList().AsQueryable();


            //return mapper.Map<IEnumerable<BlogDto>>(filteredItems.ToList());

            var items = mapper.Map<IEnumerable<BlogDto>>(blogs.ToList()).AsQueryable();

            var filteredItems = queryOptions.ApplyTo(items) as IQueryable<BlogDto>;

            return filteredItems.ToList();
        }


        BlogDto? IBlogService.GetById(int id)
        {
            var item = blogs.FirstOrDefault(b => b.Id == id);
            return mapper.Map<BlogDto>(item);
        }

        private IQueryable<TEntity> TranslateQuery<TDto, TEntity>(IQueryable<TEntity> query, ODataQueryOptions<TDto> queryOptions)
        {
            //Todo move this to an extension of IQueryable?

            var translatedExpression = mapper.Map<Expression<Func<TDto, bool>>, Expression<Func<TEntity, bool>>>(queryOptions.GetFilter());

            if (translatedExpression != null)
            {
                query = query.Where(translatedExpression);
            }
            if (queryOptions.Top != null)
            {
                query = query.Take(queryOptions.Top.Value);
            }
            if (queryOptions.Skip != null)
            {
                query = query.Skip(queryOptions.Skip.Value);
            }
            if (queryOptions.OrderBy != null)
            {
                //TODO check this later
            }

            return query;
        }
    }
}
