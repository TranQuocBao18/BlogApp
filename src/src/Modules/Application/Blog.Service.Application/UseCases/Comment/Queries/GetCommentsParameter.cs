using System;
using Blog.Model.Dto.Shared.Filters;

namespace Blog.Service.Application.UseCases.Comment.Queries;

public class GetCommentsParameter : RequestParameter
{

}

public class SearchBlogsParameter : RequestParameter
{
    public string? Search { get; set; }
}
