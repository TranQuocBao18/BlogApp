using System;

namespace Blog.Model.Dto.Shared.Filters;

public class RequestParameter : BaseCriteria
{
    public RequestParameter()
    {
        this.PageNumber = 1;
        this.PageSize = 10;
    }
    public RequestParameter(int pageNumber, int pageSize)
    {
        this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
        this.PageSize = pageSize > 10 ? 10 : pageSize;
    }
}
