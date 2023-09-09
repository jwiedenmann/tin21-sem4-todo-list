using Microsoft.AspNetCore.Mvc;
using Pyco.Todo.Core.Authorization.Attributes;

namespace Pyco.Todo.Controllers;

[Authorize]
[Route("{controller}")]
public class TodoController : Controller
{
    [HttpGet]
    public ActionResult Index(string id)
    {
#if DEBUG
        return Redirect("http://localhost:8080/");
#else
        return File("/index.html", "text/html");
#endif

    }
}
