namespace HtmxRazorSlices.Lib;

public class SuccessResult<T> : Result<T>
{
    public SuccessResult(T data) : base(data)
    {
        Success = true;
    }
}

public class SuccessResult : Result
{
    public SuccessResult()
    {
        Success = true;
    }
}