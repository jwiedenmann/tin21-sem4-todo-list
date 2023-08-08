using Microsoft.AspNetCore.Mvc;
using Pyco.Todo.Core.Authorization.Attributes;
using Pyco.Todo.DataAccess;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("/api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    //[HttpGet]
    //public async Task<IActionResult> GetAll()
    //    => Ok(await _userRepository.GetAsync());

    [HttpGet]
    public async Task<IActionResult> GetByUsername(string username)
        => Ok(await _userRepository.GetAsync(username));
}