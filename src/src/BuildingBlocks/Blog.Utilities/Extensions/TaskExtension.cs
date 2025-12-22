using System;

namespace Blog.Utilities.Extensions;

public static class TaskExtension
{
    public static async Task WhenAll(this IEnumerable<Task> source, int initialCount = 1, int degreeOfParallelism = 1)
    {
        var tasks = new List<Task>();
        using (var throttler = new SemaphoreSlim(initialCount, degreeOfParallelism))
        {
            try
            {
                await throttler.WaitAsync();
                tasks.AddRange(source);
                await Task.WhenAll(tasks);

                await Task.WhenAll(source);
            }
            finally
            {
                throttler?.Release();
            }
        }
        ;
    }
}
