namespace HtmxRazorSlices.Lib;

public class Error(string? code, string details)
{
    public Error(string details) : this(null, details)
    {
    }

    public string Code { get; } = string.Empty;
    public string Details { get; } = string.Empty;
}