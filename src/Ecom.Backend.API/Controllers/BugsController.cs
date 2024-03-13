using Ecom.Backend.API.Errors;
using Ecom.Backend.InfraStructure.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BugsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("NotFound")]
        public ActionResult GetNotFound()
        {
            var product =  _context.Products.Find(5000);
            if( product is null) 
            {
                return NotFound(new CommonResponseError(404));
            }
            return Ok(product);

        }
        [HttpGet]
        [Route("ServerError")]
        public ActionResult GetServerError()
        {
            var product =  _context.Products.Find(5000);
            product.Description = "Error";
            return Ok();

        }
        [HttpGet]
        [Route("BadRequestData/{id}")]
        public ActionResult GetBadRequestData(int id)
        {
            return Ok();
        }

        [HttpGet]
        [Route("BadRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new CommonResponseError(400));
        }
    }
}
