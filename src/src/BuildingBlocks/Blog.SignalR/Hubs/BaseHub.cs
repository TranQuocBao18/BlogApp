using System;
using Blog.Shared.Auth;
using Blog.SignalR.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Blog.SignalR.Hubs;

public abstract class BaseHub : Hub
{
    protected IOnlineClientManager OnlineClientManager { get; }
    protected IOnlineClientProvider ClientProvider { get; }
    protected ISecurityContextAccessor SecurityContextAccessor { get; }
    protected IUserGroupManager UserGroupManager { get; }

    protected readonly ILogger _logger;

    public BaseHub(
        IOnlineClientManager onlineClientManager,
        IUserGroupManager userGroupManager,
        IOnlineClientProvider clientProvider,
        ISecurityContextAccessor securityContextAccessor,
        ILogger logger
    )
    {
        OnlineClientManager = onlineClientManager;
        UserGroupManager = userGroupManager;
        ClientProvider = clientProvider;
        SecurityContextAccessor = securityContextAccessor;
        _logger = logger;
    }

    //Called when a new connection is established with the hub.
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst("uid")?.Value;
        _logger.LogInformation($"=== SignalR OnConnectedAsync ===");
        _logger.LogInformation($"ConnectionId: {Context.ConnectionId}");
        _logger.LogInformation($"UserId from token (uid): {userId}");
        _logger.LogInformation($"IsAuthenticated: {Context.User?.Identity?.IsAuthenticated}");
        _logger.LogInformation($"All claims: {string.Join(", ", Context.User?.Claims?.Select(c => $"{c.Type}={c.Value}") ?? Array.Empty<string>())}");

        await base.OnConnectedAsync();
        var client = CreateClient();
        _logger.LogInformation($"A client is connected, client = {JsonConvert.SerializeObject(client)}");

        //Add user to his groups
        if (!string.IsNullOrWhiteSpace(client?.UserId))
        {
            var groups = UserGroupManager.GetGroups(client);
            foreach (var group in groups)
            {
                await Groups.AddToGroupAsync(client.ConnectionId, group.Name);
            }
        }
        OnlineClientManager.Add(client);
    }

    //Called when a connection with the hub is terminated.
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
        var client = CreateClient();
        _logger.LogDebug("A client is disconnected", client);

        try
        {
            OnlineClientManager.Remove(Context.ConnectionId);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex.Message, ex);
        }
    }

    protected virtual IOnlineClient CreateClient()
    {
        return ClientProvider.CreateClientFromCurrentConnection(this.Context);
    }
}
