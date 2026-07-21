using ECommerce.Application.Features.Common.Behaviors.Errors;
using ECommerce.Application.Features.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Extensions
{
    public class ResultExtensions
    {
        public static IActionResult ToActionResult(Result result)
        {
            if (result.IsSuccess)
            {
                return new NoContentResult();
            }

            return result.Error.Type switch
            {
                ErrorType.NotFound => new NotFoundObjectResult(new { error = result.Error.Message }),
                ErrorType.Validation => new BadRequestObjectResult(new { error = result.Error.Message }),
                ErrorType.Conflict => new ConflictObjectResult(new { error = result.Error.Message }),
                ErrorType.Unauthorized => new UnauthorizedObjectResult(new { error = result.Error.Message }),
                _ => new ObjectResult(new { error = "Server error" }) { StatusCode = 500 }
            };
        }

        public static IActionResult ToActionResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(result.Value);
            }

            return result.Error.Type switch
            {
                ErrorType.NotFound => new NotFoundObjectResult(new { error = result.Error.Message }),
                ErrorType.Validation => new BadRequestObjectResult(new { error = result.Error.Message }),
                ErrorType.Conflict => new ConflictObjectResult(new { error = result.Error.Message }),
                ErrorType.Unauthorized => new UnauthorizedObjectResult(new { error = result.Error.Message }),
                _ => new ObjectResult(new { error = "Server error" }) { StatusCode = 500 }
            };
        }
    }
}
