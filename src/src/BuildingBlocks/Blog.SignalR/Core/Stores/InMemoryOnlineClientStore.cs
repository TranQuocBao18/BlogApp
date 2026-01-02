using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Blog.SignalR.Core.Stores;

public class InMemoryOnlineClientStore : IOnlineClientStore
{
    protected ConcurrentDictionary<string, IOnlineClient> Clients { get; }
    public InMemoryOnlineClientStore()
    {
        Clients = new ConcurrentDictionary<string, IOnlineClient>();
    }

    public void Add(IOnlineClient client)
    {
        Clients.AddOrUpdate(client.ConnectionId, client, (s, o) => client);
    }

    public IOnlineClient Get(string connectionId)
    {
        Clients.TryGetValue(connectionId, out var client);
        return client;
    }

    public bool TryRemove(string connectionId, out IOnlineClient client)
    {
        return Clients.TryRemove(connectionId, out client);
    }

    public bool Contains(string connectionId)
    {
        return Clients.ContainsKey(connectionId);
    }

    public IReadOnlyList<IOnlineClient> GetAll()
    {
        return Clients.Values.ToImmutableList();
    }
}
