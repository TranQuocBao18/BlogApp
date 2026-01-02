using System;
using Blog.Shared.Auth;
using Microsoft.AspNetCore.SignalR;

namespace Blog.SignalR.Core;

public interface IOnlineClientProvider
{
    IOnlineClient CreateClientFromCurrentConnection(HubCallerContext context);
}

public class OnlineClientProvider : IOnlineClientProvider
{
    private readonly IClientInfoProvider _clientInfoProvider;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public OnlineClientProvider(IClientInfoProvider clientInfoProvider, ISecurityContextAccessor securityContextAccessor)
    {
        _clientInfoProvider = clientInfoProvider;
        _securityContextAccessor = securityContextAccessor;
    }

    public IOnlineClient CreateClientFromCurrentConnection(HubCallerContext context)
    {
        var client = new OnlineClient(context.ConnectionId, GetIpAddress());
        SetUserInfo(client);
        return client;
    }

    private void SetUserInfo(IOnlineClient client)
    {
        client.UserId = _securityContextAccessor.UserId.ToString();
    }

    private string GetIpAddress()
    {
        try
        {
            return _clientInfoProvider.IpAddress;
        }
        catch
        {
            return string.Empty;
        }
    }
}
