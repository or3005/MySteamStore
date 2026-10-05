using System.Collections.Concurrent;

public interface IConnectionTracker
{
    void Add(Guid userId, string connectionId);
    void Remove(Guid userId, string connectionId);
    string? GetConnectionId(Guid userId);
}

public class ConnectionTracker : IConnectionTracker
{
    private readonly ConcurrentDictionary<Guid, string> _connections = new();

    public void Add(Guid userId, string connectionId)
        => _connections[userId] = connectionId;

    public void Remove(Guid userId, string connectionId)
       => _connections.TryRemove(userId, out _);

    public string? GetConnectionId(Guid userId)
        => _connections.TryGetValue(userId, out var id) ? id : null;
}
