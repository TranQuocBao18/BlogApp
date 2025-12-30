using System;
using Blog.Model.Dto.Shared.Filters;

namespace Blog.Service.Application.UseCases.Category.Queries;

public class GetCategoriesParameter : RequestParameter
{

}

public class SearchCategoriesParameter : RequestParameter
{
    public string? Search { get; set; }
}
