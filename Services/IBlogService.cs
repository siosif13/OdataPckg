using OdataPckg.DTO;

namespace OdataPckg.Services
{
    public interface IBlogService
    {
        IEnumerable<BlogDto> Get();
        BlogDto? GetById(int id);
    }
}
