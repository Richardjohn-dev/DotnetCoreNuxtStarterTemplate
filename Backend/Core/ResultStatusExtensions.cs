using FluentValidation.Results;

namespace Backend.Core;

/// <summary>
/// Extension methods for mapping ResultStatus to HTTP status codes and titles
/// </summary>
public static class ResultStatusExtensions
{
    /// <summary>
    /// Gets the appropriate HTTP status code for a result status
    /// </summary>
    public static int GetStatusCode(this ResultStatus status) => status switch
    {
        ResultStatus.Ok => StatusCodes.Status200OK,
        ResultStatus.ValidationFailure => StatusCodes.Status400BadRequest,
        ResultStatus.NotFound => StatusCodes.Status404NotFound,
        ResultStatus.Unauthorized => StatusCodes.Status401Unauthorized,
        ResultStatus.Forbidden => StatusCodes.Status403Forbidden,
        ResultStatus.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    /// <summary>
    /// Gets an appropriate title for a result status
    /// </summary>
    public static string GetTitle(this ResultStatus status) => status switch
    {
        ResultStatus.Ok => "Success",
        ResultStatus.ValidationFailure => "Validation Error",
        ResultStatus.NotFound => "NotFound",
        ResultStatus.Unauthorized => "Unauthorized",
        ResultStatus.Forbidden => "Forbidden",
        ResultStatus.InternalServerError => "Internal Server Error",
        ResultStatus.Conflict => "Conflict",
        _ => "Error"
    };

    /// <summary>
    /// Gets a type URI for a result status
    /// </summary>
    public static string GetTypeUri(this ResultStatus status) => status switch
    {
        ResultStatus.ValidationFailure => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        ResultStatus.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        ResultStatus.Unauthorized => "https://tools.ietf.org/html/rfc7235#section-3.1",
        ResultStatus.Forbidden => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
        ResultStatus.InternalServerError => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        _ => "https://tools.ietf.org/html/rfc7231#section-6.5.1"
    };

    internal static IDictionary<string, object?> ResultToExtensions<T>(this Result<T> result)
    => GetExtensions(result.Status, result.Errors, result.ValidationFailures);

    internal static IDictionary<string, object?> ResultToExtensions(this Result result)
        => GetExtensions(result.Status, result.Errors, result.ValidationFailures);

    internal static IDictionary<string, object?> GetExtensions(
        ResultStatus resultStatus,
        IEnumerable<string> resultErrors,
        List<ValidationFailure> validationFailures)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);

        // Include validation failures grouped by property
        if (resultStatus == ResultStatus.ValidationFailure && validationFailures?.Count > 0)
        {
            foreach (var group in validationFailures.GroupBy(x => x.PropertyName))
            {
                var key = FormatPropertyName(group.Key);
                errors[key] = group.Select(x => x.ErrorMessage).ToArray();
            }
        }

        errors["general"] = resultErrors.ToArray();

        return new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["errors"] = errors
        };
    }


    private static string FormatPropertyName(string propertyName) =>
        string.IsNullOrEmpty(propertyName)
            ? propertyName
            : char.ToLowerInvariant(propertyName[0]) + propertyName[1..];

}