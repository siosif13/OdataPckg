using OdataPckg.DAL;

namespace OdataPckg.DTO
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public List<PostDto> Posts { get; } = new();
    }
}
