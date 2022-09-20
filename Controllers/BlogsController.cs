using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using OdataPckg.DTO;
using OdataPckg.Services;

namespace OdataPckg.Controllers
{
    //[ApiController]           --> not activating it removes esential api verifications, activating it duplicates endpoints and conflicts with odata
    //[Route("[controller]")]
    //[ODataRouteComponent("Blogs")]
    public class BlogsController : ODataController
    {
        private readonly IBlogService blogService;

        public BlogsController(IBlogService blogService)
        {
            this.blogService = blogService;
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IEnumerable<BlogDto>> Get()
        {
            var items = blogService.Get();
            return Ok(items);
        }

        [HttpGet("/{id}")]
        [NonAction]
        public ActionResult<BlogDto> GetById(int id)
        {
            var blog = blogService.GetById(id);
            return Ok(blog);
        }

        //[HttpPatch]
        //public ActionResult Patch (Delta<Blog> delta)
        //{
        //    return Ok();
        //}
    }
}
