using Microsoft.AspNetCore.Antiforgery;

namespace HtmxRazorSlices.Features.Home;

public static class HomeFeature
{
    private const string RouteBasePath = "/";

    private const string TemplatePath = $"/Features/Home/Templates/";

    public static void RegisterHomeFeature(this WebApplication app)
    {
        app.MapGet(RouteBasePath, () => Results.Extensions.RazorSlice<Templates.Index>());

        app.MapGet($"{RouteBasePath}antiforgery", (IAntiforgery antiforgery, IHttpContextAccessor context) =>
        {
            var httpContext = context.HttpContext;
            if (httpContext == null)
                return Results.Ok("NoHttpContextAvailable");
            
            antiforgery.SetCookieTokenAndHeader(httpContext);

            var tokenSet = antiforgery.GetTokens(httpContext);

            return Results.Ok($"{tokenSet.FormFieldName}|{tokenSet.RequestToken}");
        });
    }
}