using System;
using Blog.SignalR.Core;

namespace Blog.SignalR;

public class UserGroup
{
    public string Name { get; private set; }
    public UserGroup(string name)
    {
        Name = name;
    }
}

public interface IUserGroupManager
{
    IReadOnlyList<UserGroup> GetGroups(IOnlineClient client);
}

public class DefaultUserGroupManager : IUserGroupManager
{
    public IReadOnlyList<UserGroup> GetGroups(IOnlineClient client)
    {
        return new List<UserGroup>()
        {
            new UserGroup(client.UserId)
        };
    }
}
