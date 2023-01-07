namespace SignalRPrototype.Server.Services.Interface;

public interface ISessionHandler
{
    public Task CreateSessionAsync(string connectionId);
    public Task DisconnectSessionAsync(string connectionId);
    public void ClearContext(in string connectionId, in Guid contextId);
    public void Subscribe(string connectionId, Guid[] storeIds, Guid contextId);
    public IEnumerable<(string ConnectionId, Guid ContextId)> GetSubscribers(Guid storeId);
    public int GetSubscribersCount();
}