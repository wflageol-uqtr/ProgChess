using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProChess.Server.Authorization;

public class ValidCodeCookieAttribute: AuthorizeAttribute, IAsyncAuthorizationFilter
{
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var request = context.HttpContext.Request;

        request.Cookies.TryGetValue("studentCookie", out var studentCookie);
        if (string.IsNullOrEmpty(studentCookie))
        {
            context.Result = new UnauthorizedResult();
        }
        
        return Task.CompletedTask;
    }
}