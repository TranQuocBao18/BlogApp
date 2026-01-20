using System;
using System.Collections.Immutable;
using Blog.Shared.Notification;
using Blog.SignalR.Core.Events;

namespace Blog.SignalR.Core;

public class OnlineClientManager : IOnlineClientManager
{
    public event EventHandler<OnlineClientEventArgs> ClientConnected;
    public event EventHandler<OnlineClientEventArgs> ClientDisconnected;

    protected IOnlineClientStore Store { get; }
    protected readonly object LockObj = new object();
    public OnlineClientManager(IOnlineClientStore store)
    {
        Store = store;
    }

    public void Add(IOnlineClient client)
    {
        lock (LockObj)
        {
            Store.Add(client);
            ClientConnected?.Invoke(this, new OnlineClientEventArgs(client));
        }
    }

    public IOnlineClient Get(string connectionId)
    {
        return Store.Get(connectionId);
    }

    public bool Remove(string connectionId)
    {
        lock (LockObj)
        {
            var isRemove = Store.TryRemove(connectionId, out var client);
            if (isRemove)
            {
                ClientDisconnected?.Invoke(this, new OnlineClientEventArgs(client));
            }
            return isRemove;
        }
    }

    public IReadOnlyList<IOnlineClient> GetAll()
    {
        return Store.GetAll();
    }

    public IReadOnlyList<IOnlineClient> Find(Func<IOnlineClient, bool> predicate)
    {
        var allClients = Store.GetAll();
        return allClients.Where(predicate).ToImmutableList();
    }

    public IReadOnlyList<IOnlineClient> GetByUser(IUserIdentifier user)
    {
        return Find(c => c.UserId == user.UserId);
    }
}
