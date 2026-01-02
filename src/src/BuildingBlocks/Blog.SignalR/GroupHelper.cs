using System;

namespace Blog.SignalR;

public static class GroupHelper
{
    public static string GroupByUser(string name)
    {
        return $"USER_{name}".ToUpper();
    }
}
