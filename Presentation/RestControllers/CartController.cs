using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using Domain.IUnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.RestControllers;

[Tags("Carts APIs")]
[ApiController]
[Route("carts")]
public class CartController(
    IUnitOfWorks unitOfWorks,
    IBookRepository bookRepository,
    IMapper mapper
) : ControllerBase
{
    [EndpointDescription("Get user's carts")]
    //[Authorize]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCartsByUserId(string userId)
    {
        var carts = await unitOfWorks.Carts.GetAllByUserId(Guid.Parse(userId));
        return Ok(carts);
    }

    [EndpointDescription("Create cart")]
    //[Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCartRequest request)
    {
        Cart newCart = mapper.Map<Cart>(request);
        IEnumerable<CartDetail> cartDetails = request.Items.Select(mapper.Map<CartDetail>);

        int count = await unitOfWorks.Carts.CountByUserId(Guid.Parse(request.UserId));

        if (count == 1)
        {
            var carts = await unitOfWorks.Carts.GetAllByUserId(Guid.Parse(request.UserId));
            var cart = carts.First();

            cartDetails.ToList().ForEach(c => c.CartId = cart.Id);
            await unitOfWorks.CartDetails.CreateAsync(cartDetails);
        }
        else
        {
            newCart.UserId = Guid.Parse(request.UserId);
            await unitOfWorks.Carts.CreateAsync(newCart);
        }
        await unitOfWorks.CommitAll();

        return Ok(cartDetails);
    }

    [EndpointDescription("Add cart item")]
    //[Authorize]
    [HttpPost("{cartId}")]
    public async Task<IActionResult> AddCartItem(string cartId, [FromBody] CartItemRequest request)
    {
        Cart cart =
            await unitOfWorks.Carts.GetByIdAsync(Guid.Parse(cartId))
            ?? throw new AppException(AppError.CART_NOT_FOUND, StatusCodes.Status404NotFound);

        Book book =
            await bookRepository.GetByIdAsync(Guid.Parse(request.BookId))
            ?? throw new AppException(AppError.BOOK_NOT_FOUND, StatusCodes.Status404NotFound);

        CartDetail cartDetail = await unitOfWorks.CartDetails.GetByIdAsync(
            cart.Id,
            Guid.Parse(request.BookId)
        );

        if (cartDetail != null)
        {
            cartDetail.Quantity += request.Quantity;
            cartDetail.Price = book.Price * cartDetail.Quantity;
            unitOfWorks.CartDetails.UpdateAsync(cartDetail);
            await unitOfWorks.CommitAll();
        }
        else
        {
            cartDetail = mapper.Map<CartDetail>(request);
            cartDetail.CartId = cart.Id;
            cartDetail.Price = book.Price * cartDetail.Quantity;
            await unitOfWorks.CartDetails.CreateAsync(cartDetail);
            await unitOfWorks.CommitAll();
        }
        await unitOfWorks.CommitAll();

        return Ok(cartDetail);
    }

    [EndpointDescription("Remove cart item")]
    //[Authorize]
    [HttpDelete("{cartId}/items/{bookId}")]
    public async Task<IActionResult> RemoveCartItem(string cartId, string bookId)
    {
        Cart cart =
            await unitOfWorks.Carts.GetByIdAsync(Guid.Parse(cartId))
            ?? throw new AppException(AppError.CART_NOT_FOUND, StatusCodes.Status404NotFound);

        CartDetail cartDetail =
            await unitOfWorks.CartDetails.GetByIdAsync(cart.Id, Guid.Parse(bookId))
            ?? throw new AppException(AppError.CART_NOT_FOUND, StatusCodes.Status404NotFound);

        await unitOfWorks.CartDetails.DeleteAsync(cart.Id, Guid.Parse(bookId));
        await unitOfWorks.CommitAll();

        return Ok(cartDetail);
    }
}
