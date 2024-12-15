using Application;
using Application.Contracts;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.RestControllers;

[Tags("Carts APIs")]
[ApiController]
[Route("carts")]
public class CartController(
    ICartRepository cartRepository,
    ICartDetailRepository cartDetailRepository,
    IBookRepository bookRepository,
    IMapper mapper,
    ILogger<CartController> logger,
    IStringLocalizer<SharedResource> localizer
    ) : ControllerBase
{

    [EndpointDescription("Get user's carts")]
    [ProducesResponseType(typeof(ICollection<CartResponse>), StatusCodes.Status200OK)]
    //[Authorize]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCartsByUserId(string userId)
    {
        var carts = await cartRepository.GetAllByUserId(Guid.Parse(userId));
        var response = carts.Select(mapper.Map<CartResponse>).ToList();
        response.ForEach(cart => cart.TotalPrice = cart.CartDetails.Sum(i => i.Price));
        return Ok(response);
    }

    [EndpointDescription("Create cart")]
    [ProducesResponseType(typeof(ICollection<CartResponse>), StatusCodes.Status200OK)]
    //[Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCartRequest request)
    {
        Cart newCart = mapper.Map<Cart>(request);
        IEnumerable<CartDetail> cartDetails = request.Items.Select(mapper.Map<CartDetail>);

        int count = await cartRepository.CountByUserId(Guid.Parse(request.UserId));

        if (count == 1)
        {
            var carts = await cartRepository.GetAllByUserId(Guid.Parse(request.UserId));
            var cart = carts.First();

            cartDetails.ToList().ForEach(c => c.CartId = cart.Id);
            await cartDetailRepository.CreateAsync(cartDetails);
        }
        else
        {
            newCart.UserId = Guid.Parse(request.UserId);
            await cartRepository.CreateAsync(newCart);
        }
        await cartRepository.CommitChangesAsync();

        var response = mapper.Map<CartResponse>(cartDetails);
        return Ok(response);
    }

    [EndpointDescription("Add cart item")]
    [ProducesResponseType(typeof(CartItemResponse), StatusCodes.Status200OK)]
    //[Authorize]
    [HttpPost("{cartId}")]
    public async Task<IActionResult> AddCartItem(string cartId, [FromBody] CartItemRequest request)
    {

        Cart cart = await cartRepository.GetByIdAsync(Guid.Parse(cartId)) 
            ?? throw new AppException(AppError.CART_NOT_FOUND, StatusCodes.Status404NotFound);

        Book book = await bookRepository.GetByIdAsync(Guid.Parse(request.BookId))
            ?? throw new AppException(AppError.BOOK_NOT_FOUND, StatusCodes.Status404NotFound);

        CartDetail cartDetail = await cartDetailRepository.GetByIdAsync(cart.Id, Guid.Parse(request.BookId));

        if (cartDetail != null)
        {
            cartDetail.Quantity += request.Quantity;
            cartDetail.Price = book.Price * cartDetail.Quantity;
            cartDetailRepository.UpdateAsync(cartDetail);
        }
        else
        {
            cartDetail = mapper.Map<CartDetail>(request);
            cartDetail.CartId = cart.Id;
            cartDetail.Price = book.Price * cartDetail.Quantity;
            await cartDetailRepository.CreateAsync(cartDetail);
        }
        await cartDetailRepository.CommitChangesAsync();

        var response = mapper.Map<CartItemResponse>(cartDetail);
        return Ok(response);

    }

    [EndpointDescription("Remove cart item")]
    [ProducesResponseType(typeof(CartItemResponse), StatusCodes.Status200OK)]
    //[Authorize]
    [HttpDelete("{cartId}/items/{bookId}")]
    public async Task<IActionResult> RemoveCartItem(string cartId, string bookId)
    {

        Cart cart = await cartRepository.GetByIdAsync(Guid.Parse(cartId))
            ?? throw new AppException(AppError.CART_NOT_FOUND, StatusCodes.Status404NotFound);

        CartDetail cartDetail = await cartDetailRepository.GetByIdAsync(cart.Id, Guid.Parse(bookId))
            ?? throw new AppException(AppError.CART_NOT_FOUND, StatusCodes.Status404NotFound);

        await cartDetailRepository.DeleteAsync(cart.Id, Guid.Parse(bookId));

        var response = mapper.Map<CartItemResponse>(cartDetail);
        return Ok(response);

    }

}