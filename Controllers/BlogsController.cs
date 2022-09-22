using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Query.Validator;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using OdataPckg.DTO;
using OdataPckg.Services;

namespace OdataPckg.Controllers
{
    [Route("Blogs")]
    public class BlogsController : ODataController
    {
        private readonly IBlogService blogService;

        public BlogsController(IBlogService blogService)
        {
            this.blogService = blogService;
        }

        [HttpGet]
        public ActionResult<IQueryable<BlogDto>> Get(ODataQueryOptions<BlogDto> queryOptions)
        {
            queryOptions.Validate(new ODataValidationSettings());

            var items = blogService.Get(queryOptions);

            return Ok(items);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public ActionResult<BlogDto> Get(int id, ODataQueryOptions<BlogDto> queryOptions)
        {
            var item = blogService.GetById(id);

            return Ok(item);
        }

        [HttpPost]
        [EnableQuery]
        public ActionResult<BlogDto> Create([FromBody] BlogDto blogDto)
        {
            // [Required] property on dto is not automatically validated. Do I have to explicitly call modelstate.IsValid?
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(blogService.Create(blogDto));
        }

        [HttpPut("{id}")]
        public ActionResult<BlogDto> Put(int id, [FromBody] BlogDto item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(blogService.Update(id, item));
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            blogService.Delete(id);
            return Ok();
        }
    }
}
