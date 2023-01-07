using SignalRPrototype.Shared.Enums;
using SignalRPrototype.Shared.Models.SignalR;

namespace SignalRPrototype.Shared.Extensions;

public static class SignalRExtensions
{
    public static SignalRMessage<T> ToSignalRHubMessage<T>(this T payload, SignalRMessageType type)
    {
        return new SignalRMessage<T>()
        {
            Payload = payload,
            Type = type
        };
    }
    
    public static SignalRMessage<T> ToSignalRHubMessage<T>(this T payload, SignalRMessageType type, Guid contextId)
    {
        return new SignalRMessage<T>()
        {
            Payload = payload,
            Type = type,
            ContextId = contextId
        };
    }
}