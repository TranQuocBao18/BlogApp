using System;
using Blog.Shared.Auth;
using Blog.SignalR.Core;
using Grpc.Core.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Blog.SignalR.Hubs;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CommonHub : BaseHub
{
    private readonly string OnMessageMethod = "ReceiveMessage";
    public CommonHub(
        IOnlineClientManager onlineClientManager,
        IUserGroupManager userGroupManager,
        IOnlineClientProvider clientProvider,
        ISecurityContextAccessor securityContextAccessor,
        ILogger<CommonHub> logger
    ) : base(onlineClientManager, userGroupManager, clientProvider, securityContextAccessor, logger)
    {

    }

    public async Task SendMessage(string user, string message)
    {
        _logger.LogInformation($"SendMessage from {user} : {message}");
        await Clients.All.SendAsync(OnMessageMethod, user, message);
    }

    public async Task SendMessageInstaller(string message)
    {
        _logger.LogInformation($"SendMessageInstaller message = {message}");
        await Clients.All.SendAsync($"ReceiveMessage", message);
    }
}
