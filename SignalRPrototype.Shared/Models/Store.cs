namespace SignalRPrototype.Shared.Models;

public class Store
{
    public required Guid StoreId { get; init; }
    public required string Name { get; init; }
    public int Location { get; init; }
}