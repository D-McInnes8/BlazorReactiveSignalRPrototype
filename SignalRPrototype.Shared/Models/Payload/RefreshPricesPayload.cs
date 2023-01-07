namespace SignalRPrototype.Shared.Models.Payload;

public readonly record struct RefreshPricesPayload(Guid[] StoreIds, bool Subscribe)
{
    public RefreshPricesPayload(Guid storeId, bool subscribe) : this(new[] { storeId }, subscribe)
    {
    }
};