namespace HtmxRazorSlices.Lib;

public static class HttpContextExtensions
{
    public static void Trigger(this HttpContext context, string eventName)
    {
        context.Response.Headers["HX-Trigger"] = eventName;
    }
}