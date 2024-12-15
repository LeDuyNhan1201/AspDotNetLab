using System.ComponentModel.DataAnnotations;

namespace Application.Contracts;

public sealed record CreateCartRequest
{

    [Required(ErrorMessage = "Required.UserId")]
    public string UserId { get; init; }

    [Required(ErrorMessage = "Required.CartItems")]
    public CartItemRequest[] Items { get; init; }
    
};