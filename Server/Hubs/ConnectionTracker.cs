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
    {
        _connections[userId] = connectionId;
    }

    public void Remove(Guid userId, string connectionId)
    {
        // Only removes if the stored connection is still this one, so a stale
        // disconnect doesn't wipe out a newer connection after a reconnect.
        if (_connections.TryGetValue(userId, out var current) && current == connectionId)
        {
            _connections.TryRemove(userId, out _);
        }
    }
    public string? GetConnectionId(Guid userId)
    {
        if (_connections.TryGetValue(userId, out var connectionId))
        {
            return connectionId;
        }
        return null;
    }
}
