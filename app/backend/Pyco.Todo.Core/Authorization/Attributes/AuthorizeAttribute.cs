using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Pyco.Todo.Data.Models;

namespace Pyco.Todo.Core.Authorization.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly bool _redirectOnUnauthorized;

    public AuthorizeAttribute(bool redirectOnUnauthorized = false)
    {
        _redirectOnUnauthorized = redirectOnUnauthorized;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        bool allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous) return;

        // authorization
        User user = (User)context.HttpContext.Items["User"];
        if (user == null)
        {
            if (_redirectOnUnauthorized)
            {
                context.Result = new RedirectResult("/");
            }
            else
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}