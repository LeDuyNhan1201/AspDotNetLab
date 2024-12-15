namespace Application.Contracts;

public sealed record RemoveCartItemRequest
{

    public string BookId { get; init; }

};