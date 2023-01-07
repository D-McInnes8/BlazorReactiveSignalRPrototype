namespace SignalRPrototype.Shared.Models.Payload;

public readonly record struct StorePricesSubscriptionPayload
{
    public required Guid ContextId { get; init; }
    public required Guid[] StoreIds { get; init; }
    public bool SendExistingPrices { get; init; }
}