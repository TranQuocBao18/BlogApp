using System;

namespace Blog.Model.Dto.Shared.Filters;

public class BaseCriteria
{
    public int PageNumber { get; set; } = 0;
    public int PageSize { get; set; } = 10;

    public string[]? Fields { get; set; }
    public string? Filter { get; set; }
    public string? Sorts { get; set; } = "id=desc";
}
