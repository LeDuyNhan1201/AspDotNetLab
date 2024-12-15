namespace Application.Contracts;

public sealed record CartItemResponse
{
    
    public PreviewBookResponse Book { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal Price { get; set; }

}