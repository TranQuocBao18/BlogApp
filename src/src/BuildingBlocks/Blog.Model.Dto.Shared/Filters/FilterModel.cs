using System;

namespace Blog.Model.Dto.Shared.Filters;

public class FilterModel
{
    public string QueryKey { get; set; }
    public PageSearchType QueryType { get; set; }
    public string QueryValue { get; set; }
    public string Operation { get; set; }
    public static FilterModel CreateFrom(string queryKey, PageSearchType queryType, string queryValue, string operation)
    {
        return new FilterModel()
        {
            QueryKey = queryKey,
            QueryType = queryType,
            QueryValue = queryValue,
            Operation = operation
        };
    }
}
public enum PageSearchType
{
    TEXT = 1,
    NUMBER = 2,
    BOOLEAN = 3,
    GUID = 4,
    DATE = 5,
    DATETIME = 6,
    DATETIME2 = 7,
    NULLABLE_GUID = 8,
    NULLABLE_DATE = 9,
    NULLABLE_DATETIME = 10,
    NULLABLE_DATETIME2 = 11
}
