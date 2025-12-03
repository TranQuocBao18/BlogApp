using System;

namespace Blog.Model.Dto.Shared.Filters;

public class FilterParameter
{
    public string? SearchValue { get; set; }
    public Range? Range { get; set; }
    public List<Filter>? Filters { get; set; }
}

public class Filter
{
    public required string Key { get; set; }
    public List<object>? Value { get; set; }
}

public class Range
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}
