using System.Reactive;
using Microsoft.AspNetCore.SignalR.Client;
using SignalRPrototype.Shared.Models.SignalR;

namespace SignalRPrototype.Client.Services.Concrete;

public class SignalRObservable<T> : ObservableBase<T>
{
    public Guid ContextId { get; init; }
    private HubConnection Connection { get; init; }
    
    public SignalRObservable(HubConnection connection)
    {
        ContextId = Guid.NewGuid();
        Connection = connection;
    }

    protected override IDisposable SubscribeCore(IObserver<T> observer)
    {
        return Connection.On<SignalRMessage<T>>("ReceiveMessage", (message) =>
        {
            if (message.ContextId is not null && message.ContextId != ContextId)
                return;
            observer.OnNext(message.Payload);
        });
    }
}