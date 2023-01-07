namespace SignalRPrototype.Shared.Models.Payload;

public readonly record struct SignalRClientSendMessagePayload<T>
{
    public required Guid ContextId { get; init; }
    public required T Payload { get; init; }
}