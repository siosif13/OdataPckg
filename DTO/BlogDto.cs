using Microsoft.OData.ModelBuilder;
using System.ComponentModel.DataAnnotations;

namespace OdataPckg.DTO
{
    [AutoExpand]
    public class BlogDto
    {
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
        public List<PostDto> Posts { get; } = new();
    }
}
