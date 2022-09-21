using Microsoft.AspNetCore.OData.Query;
using OdataPckg.DTO;

namespace OdataPckg.Services
{
    public interface IBlogService
    {
        IEnumerable<BlogDto> Get(ODataQueryOptions<BlogDto> queryOptions);
        BlogDto? GetById(int id);
    }
}
