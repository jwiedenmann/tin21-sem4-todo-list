using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Pyco.Todo.Core.Authorization.Attributes;
using Pyco.Todo.Data.Models;
using Pyco.Todo.DataAccess.Interfaces;

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
    public IActionResult GetByUsername(string username)
        => Ok(_userRepository.Get(username));

    [HttpGet("search")]
    public IActionResult GetByUsernameSearch(string searchTerm)
        => Ok(new List<User>() { new User() { Username = "blub"}, new User() { Username = "Jeff" } });

    [AllowAnonymous]
    [HttpGet("exists")]
    public IActionResult UsernameExists(string username)
        => Ok(_userRepository.UsernameExists(username));

    [AllowAnonymous]
    [HttpPost]
    public IActionResult CreateUser(User user)
    {
        PasswordHasher hasher = new();
        user.Password = hasher.HashPassword(user.Password);
        int? userId = _userRepository.Insert(user);

        return userId != null && userId > 0
            ? Ok()
            : BadRequest();
    }
}