using System;

namespace Blog.SignalR.Core;

public interface IOnlineClientStore
{
    void Add(IOnlineClient client);
    IOnlineClient Get(string connectionId);
    bool TryRemove(string connectionId, out IOnlineClient client);
    bool Contains(string connectionId);
    IReadOnlyList<IOnlineClient> GetAll();
}
