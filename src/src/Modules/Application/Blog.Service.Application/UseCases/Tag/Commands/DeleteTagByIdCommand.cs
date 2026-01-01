using System;
using Blog.Infrastructure.Shared.Wrappers;
using MediatR;

namespace Blog.Service.Application.UseCases.Tag.Commands;

public class DeleteTagByIdCommand : IRequest<Response<bool>>
{
    public Guid? Id { get; set; }
}
