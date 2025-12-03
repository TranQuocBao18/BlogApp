using System;
using Blog.Model.Dto.Shared.Filters;

namespace Blog.Service.Identity.UseCases.Users.Queries;

public class GetUsersParameter : RequestParameter
{

}

public class SearchUsersParameter : RequestParameter
{
    public string? Search { get; set; }
}
