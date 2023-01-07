using SignalRPrototype.Shared.Extensions;
using SignalRPrototype.Server.Services.Interface;
using SignalRPrototype.Server.Utility;
using SignalRPrototype.Shared.Enums;

namespace SignalRPrototype.Server.HostedServices;

public class AzureQueueHostedService : IHostedService, IDisposable
{
    private Timer? _timer;
    private int _executionCount;
    private readonly ISignalRService _signalRService;
    private readonly ISessionHandler _sessionHandler;

    public AzureQueueHostedService(ISignalRService signalRService, ISessionHandler sessionHandler)
    {
        _signalRService = signalRService;
        _sessionHandler = sessionHandler;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(Execute, null, TimeSpan.Zero,
            TimeSpan.FromMilliseconds(500));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private void Execute(object? state)
    {
        var count = Interlocked.Increment(ref _executionCount);
        //Console.WriteLine($"Execution Count: {count}");
        
        _ = Task.Run(async () =>
        {
            await ExecuteAsync(state);
        });
    }

    private async Task ExecuteAsync(object? state)
    {
        var productPrice = ProductHelper.GenerateProductPrice();
        var subscribers = _sessionHandler.GetSubscribers(productPrice.StoreId);

        var subscribersCount = subscribers.Select(s => s.ContextId).Count();
        var connectionsCount = subscribers.Select(s => s.ConnectionId).Distinct().Count();
        var totalConnections = _sessionHandler.GetSubscribersCount();

        if (subscribers.Any())
        {
            Console.WriteLine($"{DateTime.UtcNow}: sent price to {subscribersCount} subscribers out of {connectionsCount} connections from a total of {totalConnections} connections.");
            foreach (var (subscriber, contextId) in subscribers)
            {
                var message = productPrice.ToSignalRHubMessage(SignalRMessageType.RefreshPrices, contextId);
                await _signalRService.SendMessageToHubAsync(message, subscriber);
            }
        }
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}