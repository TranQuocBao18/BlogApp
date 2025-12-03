using System;
using Blog.Model.Dto.Shared.Filters;

namespace Blog.Service.Identity.UseCases.Roles.Queries;

public class GetRolesParameter : RequestParameter
{

}

public class SearchRolesParameter : RequestParameter
{
    public string? Search { get; set; }
}
