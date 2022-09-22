using AutoMapper;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using OdataPckg.DAL;
using OdataPckg.DAL.Entities;
using OdataPckg.DTO;
using OdataPckg.Extensions;
using System.Linq.Expressions;

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

            var query = TranslateQuery(initialQ, queryOptions);

            var items = mapper.Map<IEnumerable<BlogDto>>(query.ToList());

            // reapplying the query options seems to solve my problem of empty list
            queryOptions.ApplyTo(items.AsQueryable());

            return items;
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
