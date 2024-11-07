using System.Diagnostics;

namespace TodosApi.ApiEndpoints;

internal static class HttpResultExtensions
{
    public static IResult ToOkOrErrorHttpResult<T>(this Result<T> result) =>
        result.IsSuccess ? Results.Ok(result.OkValue) : result.Error.ToHttpResult();

    public static IResult ToHttpResult(this Error error) =>
        error switch
        {
            NotFoundError => Results.NotFound(),
            ValidationError v => Results.ValidationProblem(
                v.Failures.GroupBy(failure => failure.PropertyName)
                    .ToDictionary(group => group.Key, group => group.Select(failure => failure.ErrorMessage).ToArray())
            ),
            _ => throw new UnreachableException(),
        };
}
