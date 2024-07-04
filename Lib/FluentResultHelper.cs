using FluentResults;

namespace HtmxRazorSlices.Lib;

public static class FluentResultHelper
{
    public static IResult ToErrorResponse(this ResultBase result)
    {
        return Results.BadRequest($"{string.Join(" ", result.Errors.Select(error => error.Message).ToList())}");
    }
}