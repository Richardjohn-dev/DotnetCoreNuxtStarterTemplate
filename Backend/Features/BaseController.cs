using Backend.Core;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Shared;


[ApiController]
public class BaseController : ControllerBase
{

    /// <summary>
    /// Converts a Result object to an appropriate IActionResult
    /// </summary>
    protected IActionResult FromResult<T>(Result<T> result)
    {
        return result.IsSuccess
            ? Ok(ApiResponse<T>.Success(result.Value))
            : ProblemResult(result);
    }

    /// <summary>
    /// Converts a Result object with no value to an appropriate IActionResult
    /// </summary>
    protected IActionResult FromResult<T>(Result result)
    {
        return result.IsSuccess
            ? Ok(ApiResponse<T>.Success())
            : ProblemResult(result);
    }

    /// <summary>
    /// Creates an error result from a Result object
    /// </summary>
    protected IActionResult ProblemResult<T>(Result<T> result)
    {
        var problemDetails = result.ToProblemResult();
        problemDetails.Instance = $"{HttpContext.Request.Method} {HttpContext.Request.Path}";
        problemDetails.Extensions.TryAdd("requestId", HttpContext.TraceIdentifier);

        var activity = HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        problemDetails.Extensions.TryAdd("traceId", activity?.Id);

        return StatusCode(GetStatusCode(result.Status), problemDetails);
    }


    /// <summary>
    /// Creates an error result from a Result object
    /// </summary>
    protected IActionResult ProblemResult(Result result)
    {
        var problemDetails = result.ToProblemResult();
        problemDetails.Instance = $"{HttpContext.Request.Method} {HttpContext.Request.Path}";
        problemDetails.Extensions.TryAdd("requestId", HttpContext.TraceIdentifier);

        var activity = HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        problemDetails.Extensions.TryAdd("traceId", activity?.Id);

        return StatusCode(GetStatusCode(result.Status), problemDetails);
    }

    private int GetStatusCode(ResultStatus status) => status switch
    {
        ResultStatus.Ok => StatusCodes.Status200OK,
        ResultStatus.ValidationFailure => StatusCodes.Status400BadRequest,
        ResultStatus.NotFound => StatusCodes.Status404NotFound,
        ResultStatus.Unauthorized => StatusCodes.Status401Unauthorized,
        ResultStatus.Forbidden => StatusCodes.Status403Forbidden,
        ResultStatus.Conflict => StatusCodes.Status409Conflict,
        ResultStatus.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    /// <summary>
    /// Returns a created result with the specified resource
    /// </summary>
    protected IActionResult CreatedResult<T>(T value, string routeName, object routeValues)
    {
        return CreatedAtRoute(routeName, routeValues, ApiResponse<T>.Success(value));
    }

    protected IActionResult NoContentResult()
    {
        return NoContent();
    }

}
