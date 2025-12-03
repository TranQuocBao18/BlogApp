using System;

namespace Blog.Model.Dto.Shared.Filters;

public class QueryFilter
{
    public string Operation { get; set; }
    public string QueryKey { get; set; }
    public QueryType QueryType { get; set; }
    public string QueryValue { get; set; }
}

public enum QueryType
{
    TEXT = 1,
    NUMBER = 2,
    BOOLEAN = 3,
    GUID = 4,
    DATE = 5,
    DATETIME = 6,
    DATETIME2 = 7
}
