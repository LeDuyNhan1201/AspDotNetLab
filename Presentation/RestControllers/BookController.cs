using System.Threading.Tasks;
using Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.RestControllers;

[Tags("Books APIs")]
[ApiController]
[Route("books")]
public class BookController(IBookRepository bookRepository) : ControllerBase
{
    [EndpointDescription("Get all books")]
    //[Authorize]
    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var books = await bookRepository.GetAllAsync();
        return Ok(books);
    }
}
