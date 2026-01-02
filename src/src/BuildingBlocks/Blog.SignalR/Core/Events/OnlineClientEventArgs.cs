using System;

namespace Blog.SignalR.Core.Events;

public class OnlineClientEventArgs : EventArgs
{
    public IOnlineClient Client { get; }
    public OnlineClientEventArgs(IOnlineClient client)
    {
        Client = client;
    }
}
