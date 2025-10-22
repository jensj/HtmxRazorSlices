namespace HtmxRazorSlices.Data;

public interface IUserIdentifierService
{
    string GetOrCreateUserId(HttpContext context);
}
