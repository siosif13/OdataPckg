using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Query.Validator;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using OdataPckg.DTO;
using OdataPckg.Services;

namespace OdataPckg.Controllers
{
    public class BlogsController : ODataController
    {
        private readonly IBlogService blogService;

        public BlogsController(IBlogService blogService)
        {
            this.blogService = blogService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BlogDto>> Get(ODataQueryOptions<BlogDto> queryOptions)
        {
            queryOptions.Validate(new ODataValidationSettings());

            var items = blogService.Get(queryOptions);

            return Ok(items);
        }

        //[HttpGet]
        ////[EnableQuery]
        //public ActionResult<IEnumerable<BlogDto>> Get(ODataQueryOptions<BlogDto> queryOptions)
        //{
        //    // should we validate query options??
        //    var items = blogService.GetTakeSkip(queryOptions);

        //    return Ok(items);
        //}

        //[HttpGet("{id}")]
        ////[NonAction]
        //public ActionResult<BlogDto> GetById(int id)
        //{
        //    var blog = blogService.GetById(id);
        //    return Ok(blog);
        //}

        //[HttpPatch]
        //public ActionResult Patch(Delta<BlogDto> delta)
        //{
        //    return Ok();
        //}
    }
}
