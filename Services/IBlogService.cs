using Microsoft.AspNetCore.OData.Query;
using OdataPckg.DTO;

namespace OdataPckg.Services
{
    public interface IBlogService
    {
        IEnumerable<BlogDto> Get(ODataQueryOptions<BlogDto> queryOptions);
        BlogDto? GetById(int id);
        BlogDto Create(BlogDto item);
        BlogDto Update(int id, BlogDto item);
        void Delete(int id);
    }
}
