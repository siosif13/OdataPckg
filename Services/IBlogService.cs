using OdataPckg.DAL;

namespace OdataPckg.Services
{
    public interface IBlogService
    {
        IEnumerable<Blog> Get();
        Blog? GetById(int id);
    }
}
