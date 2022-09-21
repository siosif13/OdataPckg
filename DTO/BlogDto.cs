using Microsoft.OData.ModelBuilder;

namespace OdataPckg.DTO
{
    [AutoExpand]
    public class BlogDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public List<PostDto> Posts { get; } = new();
    }
}
