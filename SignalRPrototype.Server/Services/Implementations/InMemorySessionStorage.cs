using System.Collections.Concurrent;
using SignalRPrototype.Server.Services;

namespace SignalRPrototype.Server.Services.Implementations;

public class InMemorySessionStorage : ISessionHandler
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<Guid, Guid[]>> _sessions;

    public InMemorySessionStorage()
    {
        _sessions = new ConcurrentDictionary<string, ConcurrentDictionary<Guid, Guid[]>>();
    }

    public Task CreateSessionAsync(string connectionId)
    {
        if (!_sessions.TryAdd(connectionId, new ConcurrentDictionary<Guid, Guid[]>()))
        {
            throw new ArgumentException("Unable to register connection id.", nameof(connectionId));
        }
        return Task.CompletedTask;
    }

    public Task DisconnectSessionAsync(string connectionId)
    {
        if (_sessions.ContainsKey(connectionId))
            _sessions.Remove(connectionId, out _);
        return Task.CompletedTask;
    }

    public void ClearContext(in string connectionId, in Guid contextId)
    {
        if (_sessions.TryGetValue(connectionId, out var contexts))
        {
            contexts.TryRemove(contextId, out _);
        }
    }

    public void Subscribe(string connectionId, Guid[] storeIds, Guid contextId)
    {
        if (_sessions.ContainsKey(connectionId))
        {
            _sessions[connectionId].AddOrUpdate(contextId, _ => storeIds, (_, _) => storeIds);
        }
    }

    public IEnumerable<(string, Guid)> GetSubscribers(Guid storeId)
    {
        List<(string, Guid)> subscribers = new();
        foreach (var (connId, sessionContexts) in _sessions)
        {
            foreach (var (contextId, subscribedStores) in sessionContexts)
            {
                if (subscribedStores.Contains(storeId))
                {
                    subscribers.Add((connId, contextId));
                }
            }
        }
        return subscribers;
    }

    public int GetSubscribersCount() => _sessions.Count;
}