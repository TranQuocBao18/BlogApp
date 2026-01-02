using System;
using Blog.Shared.Timing;

namespace Blog.SignalR.Core;

public interface IOnlineClient
{
    string ConnectionId { get; }
    string IpAddress { get; }
    DateTime ConnectTime { get; }
    string UserId { get; set; }
    IDictionary<string, object> Properties { get; }
    object this[string key] { get; set; }
}

public class OnlineClient : IOnlineClient
{
    public OnlineClient(string connectionId, string ipAddress)
    {
        ConnectTime = TimeClock.Now;
        ConnectionId = connectionId;
        IpAddress = ipAddress;
        Properties = new Dictionary<string, object>();
    }

    public string ConnectionId { get; set; }
    public string IpAddress { get; set; }
    public DateTime ConnectTime { get; set; }
    public IDictionary<string, object> Properties
    {
        get { return _properties; }
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "Can't assign null to OnlineClient.Properties");
            }
            _properties = value;
        }
    }
    public string UserId { get; set; }
    private IDictionary<string, object> _properties;
    public object this[string key]
    {
        get { return Properties[key]; }
        set { Properties[key] = value; }
    }
}
