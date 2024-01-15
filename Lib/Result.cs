namespace HtmxRazorSlices.Lib;

public abstract class Result<T>(T? data) : Result
{
    public T? Data
    {
        get => Success ? data : throw new Exception($"You can't access .{nameof(Data)} when .{nameof(Success)} is false");
        set => data = value;
    }
}

public abstract class Result
{
    public bool Success { get; protected init; }
    public bool Failure => !Success;
}