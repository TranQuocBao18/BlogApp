using System;
using Blog.Model.Dto.Shared.Filters;

namespace Blog.Service.Application.UseCases.Tag.Queries;

public class GetTagsParameter : RequestParameter
{

}

public class SearchTagsParameter : RequestParameter
{
    public string? Search { get; set; }
}
