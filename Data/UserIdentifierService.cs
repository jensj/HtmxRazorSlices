namespace HtmxRazorSlices.Data;

public class UserIdentifierService : IUserIdentifierService
{
    private const string UserIdHeader = "X-User-Id";

    public string GetOrCreateUserId(HttpContext context)
    {
        // Check if the request already has a user ID header
        if (context.Request.Headers.TryGetValue(UserIdHeader, out var userId) && !string.IsNullOrWhiteSpace(userId))
        {
            return userId.ToString();
        }

        // Generate a new user ID
        var newUserId = Guid.NewGuid().ToString("N");
        
        // Set the user ID in the response header so the client can store it
        context.Response.Headers[UserIdHeader] = newUserId;
        
        return newUserId;
    }
}
