using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.Category.Commands.Handlers;

public class UpsertCategoryHandler : IRequestHandler<UpsertCategoryCommand, Response<Guid>>
{
    private readonly ICategoryService _service;

    public UpsertCategoryHandler(ICategoryService service)
    {
        _service = service;
    }

    public Task<Response<Guid>> Handle(UpsertCategoryCommand request, CancellationToken cancellationToken)
    {
        if (request.Payload!.Id.HasValue)
        {
            return _service.UpdateCategoryAsync(request.Payload, cancellationToken);
        }
        return _service.CreateCategoryAsync(request.Payload, cancellationToken);
    }
}
