using System.Threading.Tasks;
using Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.RestControllers;

[Tags("Users APIs")]
[ApiController]
[Route("users")]
public class UserController(IUserRepository userRepository) : ControllerBase
{
    [EndpointDescription("Get all users")]
    //[Authorize]
    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var users = await userRepository.GetAllAsync();
        return Ok(users);
    }
}
