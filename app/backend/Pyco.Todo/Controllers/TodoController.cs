using Microsoft.AspNetCore.Mvc;

namespace Pyco.Todo.Controllers;

[Route("todo")]
public class TodoController : Controller
{
    // Enumerate all SPA urls so we boot the Vue app correctly
    // Alternatively, use this as the 404 page so any unhandled url will boot the vue app
    public ActionResult Index(string id)
    {
#if DEBUG
        return Redirect("http://localhost:8080/");
#else
        return File("/index.html", "text/html");
#endif

    }
}
