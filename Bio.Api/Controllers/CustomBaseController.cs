using BlogProject.Application.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        [NonAction]
        public IActionResult CreateActionResult<T>(ServiceResult<T> result)
        {
            return result.StatusCode switch
            {
                HttpStatusCode.NoContent => NoContent(), 
                HttpStatusCode.Created => CreatedAtAction(
                    actionName: null,
                    routeValues: null,
                    value: result.Data),
                HttpStatusCode.BadRequest => BadRequest(result.ErrorMessage), 
                HttpStatusCode.NotFound => NotFound(result.ErrorMessage), 
                _ => new ObjectResult(result) { StatusCode = (int)result.StatusCode }
            };
        }

        [NonAction]
        public IActionResult CreateActionResult(ServiceResult result)
        {
            return result.Status switch
            {
                HttpStatusCode.NoContent => NoContent(),
                HttpStatusCode.BadRequest => BadRequest(result.ErrorMessage),
                HttpStatusCode.NotFound => NotFound(result.ErrorMessage),
                _ => new ObjectResult(result) { StatusCode = (int)result.Status }
            };
        }
    }
}
