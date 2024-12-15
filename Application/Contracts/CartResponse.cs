using System;

namespace Application.Contracts;

public sealed record CartResponse
{
    public Guid Id { get; set; }

    public string User { get; set; }

    public CartItemResponse[] CartDetails { get; set; }

    public decimal TotalPrice { get; set; }

}