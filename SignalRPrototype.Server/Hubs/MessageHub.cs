using Microsoft.AspNetCore.SignalR;
using SignalRPrototype.Shared.Extensions;
using SignalRPrototype.Server.Services;
using SignalRPrototype.Server.Utility;
using SignalRPrototype.Shared.Enums;
using SignalRPrototype.Shared.Models;
using SignalRPrototype.Shared.Models.Payload;
using SignalRPrototype.Shared.Models.SignalR;

namespace SignalRPrototype.Server.Hubs;

public class MessageHub : Hub
{
    private readonly ISessionHandler _sessionHandler;

    public MessageHub(ISessionHandler sessionHandler)
    {
        _sessionHandler = sessionHandler;
    }
    
    public override async Task OnConnectedAsync()
    {
        await _sessionHandler.CreateSessionAsync(Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _sessionHandler.DisconnectSessionAsync(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    [HubMethodName("RefreshPrices")]
    public async Task<SignalRMessage<ProductPrice[]>> RefreshPrices(SignalRMessage<RefreshPricesPayload> payload)
    {
        switch(payload.Type) {
            case SignalRMessageType.RefreshPrices:
                if (payload.Payload.Subscribe is true && payload.ContextId is { } contextId)
                {
                    _sessionHandler.Subscribe(Context.ConnectionId, payload.Payload.StoreIds, contextId);
                }
                return ProductHelper.GenerateProductPricesForStoreIds(payload.Payload.StoreIds)
                    .ToSignalRHubMessage(SignalRMessageType.RefreshPrices);
            default:
                throw new ArgumentOutOfRangeException(nameof(payload.Type));
        }
    }

    // [HubMethodName("UpdateContextStores")]
    // public void UpdateContextStores(Guid contextId, Guid[] storeIds)
    // {
    //     _sessionHandler.Subscribe(Context.ConnectionId, storeIds, contextId);
    // }

    [HubMethodName("SubscribeToStores")]
    public ProductPrice[] SubscribeToStores(SignalRClientSendMessagePayload<StorePricesSubscriptionPayload> payload)
    {
        _sessionHandler.Subscribe(Context.ConnectionId, payload.Payload.StoreIds, payload.ContextId);
        return payload.Payload.SendExistingPrices 
            ? ProductHelper.GenerateProductPricesForStoreIds(payload.Payload.StoreIds) 
            : Array.Empty<ProductPrice>();
    }

    [HubMethodName("ClearContext")]
    public void ClearContext(Guid contextId)
    {
        _sessionHandler.ClearContext(Context.ConnectionId, contextId);
    }
}