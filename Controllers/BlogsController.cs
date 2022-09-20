using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using OdataPckg.DAL;
using OdataPckg.Services;

namespace OdataPckg.Controllers
{
    //[ApiController]           --> not activating it removes esential api verifications, activating it duplicates endpoints and conflicts with odata
    //[Route("[controller]")]
    //[ODataRouteComponent("Blogs")]
    public class BlogsController : ODataController
    {
        private readonly IBlogService blogService;
        private readonly BloggingContext context;

        public BlogsController(IBlogService blogService, BloggingContext context)
        {
            this.blogService = blogService;
            this.context = context;
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IEnumerable<Blog>> Get()
        {
            return Ok(context.Set<Blog>());
        }

        [HttpGet("/{id}")]
        [NonAction]
        public ActionResult<Blog> GetById(int id)
        {
            var blog = blogService.GetById(id);
            return Ok(blog);
        }

        [HttpGet("blabla")]
        public ActionResult<IEnumerable<Blog>> Bla()
        {
            return Ok(context.Set<Blog>());
        }

        [HttpPatch]
        public ActionResult Patch (Delta<Blog> delta)
        {
            return Ok();
        }
    }
}
