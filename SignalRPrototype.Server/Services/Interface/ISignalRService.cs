using SignalRPrototype.Shared.Models;
using SignalRPrototype.Shared.Models.SignalR;

namespace SignalRPrototype.Server.Services.Interface;

public interface ISignalRService
{
    public Task SendRefreshPricesMessageAsync(SignalRMessage<ProductPrice[]> message);
    public Task SendMessageToHubAsync<T>(SignalRMessage<T> message);
    public Task SendMessageToHubAsync<T>(SignalRMessage<T> message, string connectionId);
    public Task SendMessageToHubAsync<T>(SignalRMessage<T> message, string[] connectionIds);
}