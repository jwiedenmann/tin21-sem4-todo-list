using Microsoft.AspNetCore.Mvc;

namespace Pyco.Todo.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class TestController : Controller
    {
        public int Index()
        {
            return 42;
        }
    }
}
