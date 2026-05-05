using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiSoto.Application.Common.Models;

namespace WebApiSoto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        private readonly Dictionary<int, Func<object, IActionResult>> _statusCodeHandler;
       protected ApiController() {
            _statusCodeHandler = new Dictionary<int, Func<object, IActionResult>>
            {
                { 200, data => Ok(data)},
                { 204, data => NoContent()},
                { 400, err => BadRequest(err)},
                { 401, err => Unauthorized(err) },
                { 404, err=> NotFound(err) },
                { 409, err => Conflict(err)}
            };
        }

       protected IActionResult HandleResult<T> (Result<T> result)
        {
            if(_statusCodeHandler.TryGetValue(result.StatusCode, out var handler))
            {
                return handler(result.IsSuccess ? result : (object)result);
            }

            return StatusCode(500, result);
        }
    }
}
