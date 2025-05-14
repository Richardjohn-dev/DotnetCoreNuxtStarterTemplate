using Microsoft.AspNetCore.Mvc;

namespace Backend.Core;

public static class ResultExtensions
{
    public static ProblemDetails ToProblemResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException();

        return new ProblemDetails
        {
            Status = result.Status.GetStatusCode(),
            Title = result.Status.GetTitle(),
            Type = result.Status.GetTypeUri(),
            Extensions = result.ResultToExtensions(),
        };
    }

    public static ProblemDetails ToProblemResult(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot cast success to problem details");
        }

        return new ProblemDetails
        {
            Status = result.Status.GetStatusCode(),
            Title = result.Status.GetTitle(),
            Type = result.Status.GetTypeUri(),
            Extensions = result.ResultToExtensions(),
        };


    }



}
