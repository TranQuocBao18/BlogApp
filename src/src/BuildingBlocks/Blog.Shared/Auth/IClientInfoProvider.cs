using System;

namespace Blog.Shared.Auth;

public interface IClientInfoProvider
{
    string BrowserInfo { get; }
    string IpAddress { get; }
}
