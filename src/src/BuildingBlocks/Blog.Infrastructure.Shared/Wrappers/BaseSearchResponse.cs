using System;
using Blog.Model.Dto.Shared.Filters;

namespace Blog.Infrastructure.Shared.Wrappers;

public class BaseSearchResponse<T> : Response<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public long TotalItems { get; set; }
    private List<T> _data;
    public IEnumerable<T> Data => _data;
    public BaseSearchResponse()
    {
        _data = new List<T>();
    }
    public void AddRangeData(IEnumerable<T> data)
    {
        _data.AddRange(data);
    }
    public void Add(T item)
    {
        _data.Add(item);
    }

    public BaseSearchResponse(IEnumerable<T> data, int pageNumber, int pageSize, long totalItems)
    {
        this.TotalItems = totalItems;
        this.PageNumber = pageNumber;
        this.PageSize = pageSize;
        this.Succeeded = true;
        this.Message = null;
        this.Errors = null;
        _data = new List<T>(data);
    }

    public static BaseSearchResponse<T> CreateFrom(IEnumerable<T> data, BaseCriteria criteria, long totalItems)
    {
        return new BaseSearchResponse<T>(data, criteria.PageSize, criteria.PageNumber, totalItems);
    }
}
