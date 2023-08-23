using Microsoft.AspNetCore.Mvc;
using Pyco.Todo.Core.Authorization.Attributes;

namespace Pyco.Todo.Controllers
{
    [Authorize]
    [Route("/api/v1/{controller}")]
    public class ListController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
