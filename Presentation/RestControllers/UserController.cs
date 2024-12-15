using Application;
using Application.Contracts;
using AutoMapper;
using Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.RestControllers;

[Tags("Users APIs")]
[ApiController]
[Route("users")]
public class UserController(
    IUserRepository userRepository,
    IMapper mapper,
    ILogger<CartController> logger,
    IStringLocalizer<SharedResource> localizer
    ) : ControllerBase
{

    [EndpointDescription("Get all users")]
    [ProducesResponseType(typeof(ICollection<UserResponse>), StatusCodes.Status200OK)]
    //[Authorize]
    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var users = await userRepository.GetAllAsync();
        var response = users.Select(mapper.Map<UserResponse>);
        return Ok(response);
    }

}