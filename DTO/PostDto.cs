using Microsoft.OData.ModelBuilder;
using OdataPckg.DAL;

namespace OdataPckg.DTO
{
    [AutoExpand]
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

    }
}
