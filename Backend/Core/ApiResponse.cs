using FluentValidation.Results;

namespace Backend.Core;

// For successful Api Responses. 
public record ApiResponse<T>(T Payload, string? Message = null)
{
    public static ApiResponse<T> Success(T payload, string? message = "Operation successful.")
    {
        return new ApiResponse<T>(payload, message);
    }

    public static ApiResponse<bool> Success(string? message = "Operation successful.")
    {
        return new ApiResponse<bool>(true, message);
    }


}



public record Result : Result<Result>
{

    public Result()
    {

    }


    public static Result Success()
    {
        return new Result();
    }
    public static Result<T> Success<T>(T value)
    {
        return new Result<T>(value);
    }
    protected internal Result(ResultStatus status)
       : base(status)
    {
    }
    public new static Result Error(ResultStatus status, params string[] errorMessages)
    {
        return new Result(status)
        {
            Errors = errorMessages
        };
    }

    public new static Result Unauthorized(params string[] errorMessages)
    {
        return new Result(ResultStatus.Unauthorized)
        {
            Errors = errorMessages
        };
    }
    public static Result Conflict(params string[] errorMessages)
    {
        return new Result(ResultStatus.Conflict)
        {
            Errors = errorMessages
        };
    }


    public static Result InternalServerError(params string[] errorMessages)
    {
        return new Result(ResultStatus.InternalServerError)
        {
            Errors = errorMessages
        };
    }


    public static Result NotFound() => new(ResultStatus.NotFound)
    {

    };

    internal static Result ValidationFailure(List<ValidationFailure> validationResult)
    {
        return new Result(ResultStatus.ValidationFailure)
        {
            ValidationFailures = validationResult
        };
    }




}
public record Result<T> : IResult
{
    public T Value { get; }
    public ResultStatus Status { get; protected set; }
    public string SuccessMessage { get; protected set; } = string.Empty;
    public IEnumerable<string> Errors { get; protected set; } = new List<string>();
    public bool IsSuccess => Status == ResultStatus.Ok;
    public List<ValidationFailure> ValidationFailures { get; protected set; } = new List<ValidationFailure>();

    protected Result(ResultStatus status)
    {
        Status = status;
    }
    protected Result()
    {

    }

    public static Result<Tnew> TransformPayload<Told, Tnew>(Tnew newPayload, Result<Told> oldResult, ResultStatus? newStatus)
    {
        return new Result<Tnew>(newPayload)
        {
            Status = newStatus ?? oldResult.Status,
            Errors = oldResult.Errors,
            SuccessMessage = oldResult.SuccessMessage,
            ValidationFailures = oldResult.ValidationFailures,
        };
    }
    public Result(T value) => Value = value;

    public static implicit operator Result<T>(Result result)
    {
        return new Result<T>(default(T))
        {
            Status = result.Status,
            Errors = result.Errors,
            SuccessMessage = result.SuccessMessage,
            ValidationFailures = result.ValidationFailures,
        };
    }

    public static implicit operator T(Result<T> result)
    {
        return result.Value;
    }

    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(value);
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }

    public static Result SuccessWithMessage(string message)
    {
        return new Result(ResultStatus.Ok)
        {
            SuccessMessage = message
        };
    }

    public static Result<T> Error(ResultStatus status, params string[] errorMessages)
    {
        return new Result<T>(status)
        {
            Errors = errorMessages
        };
    }


    public static Result<T> NotFound(params string[] errorMessages)
    {
        return new Result<T>(ResultStatus.NotFound)
        {
            Errors = errorMessages
        };
    }

    public static Result<T> Unauthorized(params string[] errorMessages)
    {
        return new Result<T>(ResultStatus.Unauthorized)
        {
            Errors = errorMessages
        };
    }


}
public interface IResult
{
    ResultStatus Status { get; }

    IEnumerable<string> Errors { get; }

    List<ValidationFailure> ValidationFailures { get; }

}
public enum ResultStatus
{
    Ok = 0,
    //Error = 1,
    Forbidden = 2,
    Unauthorized = 3,
    Invalid = 4,
    NotFound = 5,
    Unhealthy = 6,
    InternalServerError = 7,
    DomainException = 10,
    ValidationFailure = 11,
    Conflict = 12,
}
