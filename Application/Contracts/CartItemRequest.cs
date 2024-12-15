using System.ComponentModel.DataAnnotations;

namespace Application.Contracts;

public sealed record CartItemRequest
{

    [Required(ErrorMessage = "Required.BookId")]
    public string BookId { get; init; }

    [Range(1, 10, ErrorMessage = "Range.Quantity")]
    public int Quantity { get; init; }
    
};