using System;
using Microsoft.AspNetCore.SignalR;

namespace Blog.SignalR;

public class SignalROptions
{
    public string AllowOrigins { get; set; }
    public Func<HubConnectionContext, string> GetUserIdFunc { get; set; }
}
