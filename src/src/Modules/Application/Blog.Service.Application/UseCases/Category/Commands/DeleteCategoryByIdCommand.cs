using System;
using Blog.Infrastructure.Shared.Wrappers;
using MediatR;

namespace Blog.Service.Application.UseCases.Category.Commands;

public class DeleteCategoryByIdCommand : IRequest<Response<bool>>
{
    public Guid? Id { get; set; }
}
