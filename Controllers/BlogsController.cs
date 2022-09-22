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

        //[HttpGet]
        public ActionResult<IEnumerable<BlogDto>> Get(ODataQueryOptions<BlogDto> queryOptions)
        {
            queryOptions.Validate(new ODataValidationSettings());

            var items = blogService.Get(queryOptions);

            return Ok(items);
        }

        //[HttpGet]
        public ActionResult<BlogDto> Get(int key)
        {
            // renaming key to id seems to break the method

            var item = blogService.GetById(key);
            // the problem of including Posts persists here, because we have no query options to apply, Posts are removed somehow by the OData pipeline

            return Ok(item);
        }


        //[HttpGet]     --> routing verbs are ignored here and routing is made by convention
        public ActionResult<BlogDto> Post([FromBody] BlogDto blogDto)
        {
            // if i rename Post to Create, i'll get a 405 method not allowed
            // [Required] property on dto is not automatically validated. Do I have to explicitly call modelstate.IsValid?
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(blogService.Create(blogDto));
        }

        public ActionResult<BlogDto> Put(int key, [FromBody] BlogDto item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(blogService.Update(key, item));
        }

        public ActionResult Delete(int key)
        {
            blogService.Delete(key);
            return Ok();
        }
    }
}
