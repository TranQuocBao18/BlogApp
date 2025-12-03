using System;

namespace Blog.Shared.Logging;

public class LoggingOptions
{
    public bool ConsoleEnabled { get; set; }
    public SeqOptions Seq { get; set; } = default;
}
