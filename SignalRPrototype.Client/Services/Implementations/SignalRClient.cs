using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using SignalRPrototype.Client.Services;
using SignalRPrototype.Shared.Enums;
using SignalRPrototype.Shared.Models;
using SignalRPrototype.Shared.Models.Payload;
using SignalRPrototype.Shared.Models.SignalR;

namespace SignalRPrototype.Client.Services.Implementations;

public class SignalRClient : ISignalRClient
{
    private HubConnection? _hubConnection;

    public async Task OpenConnectionAsync(Uri url)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();
        
        // _hubConnection.On<SignalRMessage<object>>("ReceiveMessage", (message) =>
        // {
        //     Console.WriteLine($"Received message of type {message.Type}");
        //     switch(message.Type) {
        //         case SignalRMessageType.RefreshPrices:
        //             break;
        //         default:
        //             throw new ArgumentOutOfRangeException(nameof(message.Type));
        //     }
        // });
        
        await _hubConnection.StartAsync();
    }
    
    public async Task CloseConnectionAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }
    }

    public Task CloseContextAsync<T>(SignalRObservable<T> context) => 
        SendMessageToHubAsync("ClearContext", context.ContextId);
    
    public async Task SendMessageToHubAsync(string methodName, Guid contextId)
    {
        if (_hubConnection is null)
            throw new InvalidOperationException("SignalR Hub Connection not opened.");
        await _hubConnection.InvokeAsync(methodName, contextId);
    }

    public async Task<TReturn> SendMessageToHubAsync<TPayload, TReturn>(string methodName, Guid contextId, TPayload payload)
    {
        if (_hubConnection is null)
            throw new InvalidOperationException("SignalR Hub Connection not opened.");
        
        return await _hubConnection.InvokeAsync<TReturn>(methodName, new SignalRClientSendMessagePayload<TPayload>
        {
            ContextId = contextId,
            Payload = payload
        });
    }

    public async Task<TReturn> SendMessageToHubAsync<TPayload, TReturn>(string methodName, SignalRMessageType type, TPayload payload,
        Guid contextId)
    {
        if (_hubConnection is null)
            throw new InvalidOperationException("SignalR Hub Connection not opened.");

        var message = new SignalRMessage<TPayload>()
        {
            Type = type,
            Payload = payload,
            ContextId = contextId
        };
        var result = await _hubConnection.InvokeAsync<SignalRMessage<TReturn>>(methodName, message);
        return result.Payload;
    }

    public async Task<(SignalRObservable<ProductPrice>, ProductPrice[])> CreatePricingObservable(RefreshPricesPayload payload)
    {
        if (_hubConnection is null)
            throw new InvalidOperationException("SignalR Hub Connection not opened.");

        var context = new SignalRObservable<ProductPrice>(_hubConnection);
        var prices = await SendMessageToHubAsync<RefreshPricesPayload, ProductPrice[]>("RefreshPrices", 
            SignalRMessageType.RefreshPrices, payload, context.ContextId);
        return (context, prices);
    }
}