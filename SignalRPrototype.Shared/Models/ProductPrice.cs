namespace SignalRPrototype.Shared.Models;

public class ProductPrice
{
    public Guid ProductId { get; init; }
    public Guid StoreId { get; init; }
    public DateTime TimeGenerated { get; init; }
    public decimal Price { get; init; }
}