using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProChess.Server.Authorization;

public class ValidCodeCookieAttribute: Attribute, IAsyncAuthorizationFilter
{
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var request = context.HttpContext.Request;
        if (!request.Cookies.TryGetValue("studentCookie", out var studentCookie))
        {
            context.Result = new UnauthorizedResult();
        }
        return Task.CompletedTask;
    }
}