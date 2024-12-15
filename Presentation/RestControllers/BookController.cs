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

[Tags("Books APIs")]
[ApiController]
[Route("books")]
public class BookController(
    IBookRepository bookRepository,
    IMapper mapper,
    ILogger<CartController> logger,
    IStringLocalizer<SharedResource> localizer
    ) : ControllerBase
{

    [EndpointDescription("Get all books")]
    [ProducesResponseType(typeof(ICollection<BookResponse>), StatusCodes.Status200OK)]
    //[Authorize]
    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var books = await bookRepository.GetAllAsync();
        var response = books.Select(mapper.Map<BookResponse>);
        return Ok(response);
    }

}