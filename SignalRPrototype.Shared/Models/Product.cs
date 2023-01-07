namespace SignalRPrototype.Shared.Models;

public readonly record struct Product
{
    public required Guid ProductId { get; init; }
    public required string Name { get; init; }
}