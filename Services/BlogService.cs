using OdataPckg.DAL;

namespace OdataPckg.Services
{
    public class BlogService : IBlogService
    {
        private readonly BloggingContext context;

        public BlogService(BloggingContext context)
        {
            this.context = context;
        }

        public IEnumerable<Blog> Get()
        {
            var items = context.Set<Blog>().ToList();
            return items;
        }

        public Blog? GetById(int id)
        {
            return context.Set<Blog>().FirstOrDefault(p => p.Id == id);
        }
    }
}
