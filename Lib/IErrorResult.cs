namespace HtmxRazorSlices.Lib;

internal interface IErrorResult
{
    string Message { get; }
    IReadOnlyCollection<Error> Errors { get; }
}