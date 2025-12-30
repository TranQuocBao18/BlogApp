using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.Category.Commands.Handlers;

public class DeleteCategoryByIdHandler : IRequestHandler<DeleteCategoryByIdCommand, Response<bool>>
{
    private readonly ICategoryService _service;

    public DeleteCategoryByIdHandler(ICategoryService service)
    {
        _service = service;
    }

    public Task<Response<bool>> Handle(DeleteCategoryByIdCommand request, CancellationToken cancellationToken)
    {
        return _service.DeleteCategoryAsync(request.Id, cancellationToken);
    }
}
