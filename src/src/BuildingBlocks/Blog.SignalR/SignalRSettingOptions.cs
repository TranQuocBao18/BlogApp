using System;

namespace Blog.SignalR;

public class SignalRSettingOptions
{
    public const string SettingKey = "SignalR";
    public bool IsBackplaneEnabled { get; set; } = false;
    public bool IsTestEndpointEnabled { get; set; } = false;
    public string Origins { get; set; } = "*";
}
