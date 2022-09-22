using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.OData.Formatter.MediaType;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using OdataPckg.DAL;
using OdataPckg.DAL.Entities;
using OdataPckg.DTO;
using OdataPckg.Extensions;
using System.Linq.Expressions;
using System.Reflection;

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
            // default includes
            var initialQ =  blogs.Include(p => p.Posts);

            var query = TranslateQuery(initialQ, queryOptions);

            var items = mapper.Map<IEnumerable<BlogDto>>(query.ToList());

            // reapplying the query options seems to solve my problem of empty list
            queryOptions.ApplyTo(items.AsQueryable());

            return items;
        }

        public BlogDto? GetById(int id)
        {
            var item = blogs.Include(p => p.Posts).FirstOrDefault(b => b.Id == id);
            return mapper.Map<BlogDto>(item);
        }

        public BlogDto Create(BlogDto item)
        {
            var entity = mapper.Map<Blog>(item);
            blogs.Add(entity);
            context.SaveChanges();

            return mapper.Map<BlogDto>(entity);
        }

        public BlogDto Update(int id, BlogDto item)
        {
            var entity = blogs.Find(id);

            blogs.Update(entity);

            context.Set<Post>().RemoveRange(context.Set<Post>().Where(p => p.BlogId == item.Id));

            mapper.Map(item, entity);

            context.SaveChanges();

            return item;
        }

        public void Delete(int id)
        {
            var entity = blogs.Find(id);
            blogs.Remove(entity);

            context.SaveChanges();
        }

        private IQueryable<TEntity> TranslateQuery<TDto, TEntity>(IQueryable<TEntity> query, ODataQueryOptions<TDto> queryOptions)
        {
            //Todo move this to an extension of IQueryable?

            if (queryOptions.Filter != null)
            {
                var translatedFilter = mapper.Map<Expression<Func<TDto, bool>>, Expression<Func<TEntity, bool>>>(
                    queryOptions.GetFilter());

                query = query.Where(translatedFilter);
            }

            if (queryOptions.OrderBy != null)
            {
                //TODO test this with different properties
                foreach (var node in queryOptions.OrderBy.OrderByNodes)
                {
                    var selectorPropertyName = (node.GetType().GetProperty("Property")?.GetValue(node, null) as EdmProperty)?.Name;
                    if (selectorPropertyName == null)
                        throw new Exception("Error mapping selector.");

                    var orderByClause = (node.GetType().GetProperty("OrderByClause")?.GetValue(node, null) as OrderByClause);
                    if (orderByClause == null)
                        throw new Exception("Error mapping clause.");

                    query = ApplyOrderBy<TDto, TEntity>(query, selectorPropertyName, orderByClause!.Direction);
                }
            }

            if (queryOptions.Skip != null)
            {
                query = query.Skip(queryOptions.Skip.Value);
            }

            if (queryOptions.Top != null)
            {
                // here I can implement a global Top limit
                query = query.Take(queryOptions.Top.Value);
            }

            return query;
        }

        private IOrderedQueryable<TEntity> ApplyOrderBy<TDto, TEntity>(IQueryable<TEntity> query, string propertyName, OrderByDirection direction)
        {
            // Get internal automapper configuration to find the related property from Dto to Entity
            var map = mapper.ConfigurationProvider.Internal().FindTypeMapFor<TDto, TEntity>();
            var propertyMap = map.PropertyMaps.FirstOrDefault(pm => pm.SourceMember.Name == propertyName);
            if (propertyMap == null)
                throw new Exception("Error finding property map.");
            var destPropertyName = propertyMap.DestinationMember.Name;

            // Get the related property on the Entity object
            var entityType = typeof(TEntity);
            var propertyInfo = entityType.GetProperty(destPropertyName);
            if (propertyInfo == null)
                throw new Exception("Error mapping properties");

            // Programatically create the order clause for the Entity type, based on the newly found selector
            var arg = Expression.Parameter(entityType); // pass "x"?
            var prop = Expression.MakeMemberAccess(arg, propertyInfo);
            var selector = Expression.Lambda(prop, new ParameterExpression[] { arg });

            var methodName = direction == OrderByDirection.Ascending
                ? "OrderBy"
                : "OrderByDescending";
            var method = typeof(Queryable).GetMethods()
                .Where(p => p.Name == methodName && p.IsGenericMethodDefinition)
                .Where(p => p.GetParameters().ToList().Count == 2)
                .Single();

            // Invoke the order by method on the queryable
            MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);
            return (IOrderedQueryable<TEntity>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
        }
    }
}