using SignalRPrototype.Shared.Enums;

namespace SignalRPrototype.Shared.Models.SignalR;

public class SignalRMessage<T>
{
    public required SignalRMessageType Type { get; init; }
    public required T Payload { get; init; }
    public Guid? ContextId { get; init; }
}