using System;
using Microsoft.AspNetCore.Http;

namespace Blog.Shared.Auth;

public class HttpContextClientInfoProvider : IClientInfoProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public HttpContextClientInfoProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string BrowserInfo => GetBrowserInfo();
    public string IpAddress => GetIpAddress();
    private string GetBrowserInfo()
    {
        return _httpContextAccessor.HttpContext?.Request?.Headers?["User-Agent"];
    }
    private string GetIpAddress()
    {
        return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
    }
}
