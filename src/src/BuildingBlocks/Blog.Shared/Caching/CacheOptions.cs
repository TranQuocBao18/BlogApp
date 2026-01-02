using System;

namespace Blog.Shared.Caching;

public class CacheOptions
{
    public bool Enabled { get; set; }
    public string Configuration { get; set; }
    public string InstanceName { get; set; }
    public int SlidingExpirationInSecond { get; set; }
}
