using System;
using Blog.Model.Dto.Shared.Filters;

namespace Blog.Service.Application.UseCases.Blog.Queries;

public class GetBlogsParameter : RequestParameter
{

}

public class SearchBlogsParameter : RequestParameter
{
    public string? Search { get; set; }
}
