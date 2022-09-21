using Microsoft.AspNetCore.OData.Query;
using System.Linq.Expressions;

namespace OdataPckg.Extensions
{
    public static class ODataQueryOptionsExtensions
    {
        public static Expression<Func<T, bool>> GetFilter<T>(this ODataQueryOptions<T> options)
        {
            IQueryable query = Enumerable.Empty<T>().AsQueryable();
            query = options.Filter?.ApplyTo(query, new ODataQuerySettings());

            var call = query?.Expression as MethodCallExpression;

            if (call != null && call.Method.Name == nameof(Queryable.Where) && call.Method.DeclaringType == typeof(Queryable))
            {
                var predicate = ((UnaryExpression)call.Arguments[1]).Operand;
                return (Expression<Func<T, bool>>)predicate;
            }
            return null;
        }
    }
}
