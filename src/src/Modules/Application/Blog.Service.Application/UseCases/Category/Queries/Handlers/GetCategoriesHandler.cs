using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Application.Responses;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.Category.Queries.Handlers;

public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, Response<IReadOnlyList<CategoryResponse>>>
{
    private readonly ICategoryService _service;

    public GetCategoriesHandler(ICategoryService service)
    {
        _service = service;
    }

    public async Task<Response<IReadOnlyList<CategoryResponse>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetCategoriesAsync(request.PageNumber, request.PageSize, request.Search, cancellationToken);
    }
}
