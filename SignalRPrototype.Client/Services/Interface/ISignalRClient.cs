using SignalRPrototype.Client.Services.Concrete;
using SignalRPrototype.Shared.Enums;
using SignalRPrototype.Shared.Models;
using SignalRPrototype.Shared.Models.Payload;

namespace SignalRPrototype.Client.Services.Interface;

public interface ISignalRClient
{
    public Task OpenConnectionAsync(Uri url);
    public Task CloseConnectionAsync();
    public Task CloseContextAsync<T>(SignalRObservable<T> context);

    public Task SendMessageToHubAsync(string methodName, Guid contextId);
    public Task<TReturn> SendMessageToHubAsync<TPayload, TReturn>(string methodName, Guid contextId, TPayload payload);
    
    public Task<(SignalRObservable<ProductPrice> Context, ProductPrice[] CurrentPrices)> RefreshPricesObservable(RefreshPricesPayload payload);
}