using Construmart.Core.DTOs.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Construmart.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class RootController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="actionName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected ActionResult ResolveActionResult<T>(T response, string actionName = null, long? id = null) where T : BaseResponse
        {
            return response.StatusCode switch
            {
                StatusCodes.Status200OK => Ok(response),
                StatusCodes.Status201Created => string.IsNullOrWhiteSpace(actionName) || !id.HasValue
                    ? StatusCode(StatusCodes.Status201Created, response as T)
                    : CreatedAtAction(actionName, new { id }, response as T),
                StatusCodes.Status204NoContent => NoContent(),
                StatusCodes.Status400BadRequest => BadRequest(response),
                StatusCodes.Status401Unauthorized => Unauthorized(response),
                StatusCodes.Status404NotFound => NotFound(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response),
            };
        }
    }
}