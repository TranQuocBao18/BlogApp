using System;
using Blog.Infrastructure.Shared.Wrappers;
using MediatR;

namespace Blog.Service.Application.UseCases.Comment.Commands;

public class DeleteCommentByIdCommand : IRequest<Response<bool>>
{
    public Guid? Id { get; set; }
}
