using System;
using Blog.Shared.Notification;
using Blog.SignalR.Core.Events;

namespace Blog.SignalR.Core;

public interface IOnlineClientManager
{
    event EventHandler<OnlineClientEventArgs> ClientConnected;
    event EventHandler<OnlineClientEventArgs> ClientDisconnected;

    void Add(IOnlineClient client);
    IOnlineClient Get(string connectionId);
    bool Remove(string connectionId);
    IReadOnlyList<IOnlineClient> GetAll();
    IReadOnlyList<IOnlineClient> Find(Func<IOnlineClient, bool> predicate);
    IReadOnlyList<IOnlineClient> GetByUser(IUserIdentifier user);
}
