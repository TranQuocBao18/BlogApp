using System;

namespace Blog.Infrastructure.Shared.Wrappers;

public class PagedResponse<T> : Response<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public long TotalItems { get; set; }

    public PagedResponse(T data, int pageNumber, int pageSize, long totalItems)
    {
        this.TotalItems = totalItems;
        this.PageNumber = pageNumber;
        this.PageSize = pageSize;
        this.Data = data;
        this.Message = null;
        this.Succeeded = true;
        this.Errors = null;
    }

    public PagedResponse(IEnumerable<T> dataList, int pageNumber, int pageSize, long totalItems)
    {
        this.TotalItems = totalItems;
        this.PageNumber = pageNumber;
        this.PageSize = pageSize;
        this.Succeeded = true;
        this.Message = null;
        this.Errors = null;
    }
}
