using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Application.Requests;
using MediatR;

namespace Blog.Service.Application.UseCases.Comment.Commands;

public partial class UpsertCommentCommand : IRequest<Response<Guid>>
{
    public CommentRequest? Payload { get; set; }
}
