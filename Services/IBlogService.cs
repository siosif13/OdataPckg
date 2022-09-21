using Microsoft.AspNetCore.OData.Query;
using OdataPckg.DTO;

namespace OdataPckg.Services
{
    public interface IBlogService
    {
        IEnumerable<BlogDto> Get(ODataQueryOptions<BlogDto> queryOptions);
        //For debug only
        IEnumerable<BlogDto> GetTakeSkip(ODataQueryOptions<BlogDto> queryOptions);
        BlogDto? GetById(int id);
    }
}
