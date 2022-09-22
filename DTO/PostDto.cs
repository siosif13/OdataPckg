using Microsoft.OData.ModelBuilder;
using OdataPckg.DAL;
using System.ComponentModel.DataAnnotations;

namespace OdataPckg.DTO
{
    [AutoExpand]
    public class PostDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }

    }
}
