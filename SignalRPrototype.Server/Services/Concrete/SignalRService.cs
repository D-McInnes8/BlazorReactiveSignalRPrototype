using Microsoft.AspNetCore.SignalR;
using SignalRPrototype.Server.Hubs;
using SignalRPrototype.Server.Services.Interface;
using SignalRPrototype.Shared.Models;
using SignalRPrototype.Shared.Models.SignalR;

namespace SignalRPrototype.Server.Services.Concrete;

public class SignalRService : ISignalRService
{
    private readonly ISessionHandler _sessionHandler;
    private readonly IHubContext<MessageHub> _hubContext;
    private readonly IHubContext<MessageHub> _anonymousHubContext;

    public SignalRService(ISessionHandler sessionHandler,
        IHubContext<MessageHub> hubContext,
        IHubContext<MessageHub> anonymousHubContext)
    {
        _hubContext = hubContext;
        _anonymousHubContext = anonymousHubContext;
        _sessionHandler = sessionHandler;
    }

    public Task SendRefreshPricesMessageAsync(SignalRMessage<ProductPrice[]> message)
    {
        return SendMessageToHubAsync(message);
    }
    
    public async Task SendMessageToHubAsync<T>(SignalRMessage<T> message)
    {
        var users = Array.Empty<string>();
        //_anonymousHubContext.Clients.All.SendAsync("ReceiveMessage", message);
        await _anonymousHubContext.Clients.Clients(users).SendAsync("ReceiveMessage", message);
    }
    
    public async Task SendMessageToHubAsync<T>(SignalRMessage<T> message, string connectionId)
    {
        await _anonymousHubContext.Clients.Clients(connectionId).SendAsync("ReceiveMessage", message);
    }
    
    public async Task SendMessageToHubAsync<T>(SignalRMessage<T> message, string[] connectionIds)
    {
        await _anonymousHubContext.Clients.Clients(connectionIds).SendAsync("ReceiveMessage", message);
    }
}