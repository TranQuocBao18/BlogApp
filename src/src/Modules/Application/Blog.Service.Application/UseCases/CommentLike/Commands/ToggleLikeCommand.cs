using System;
using Blog.Infrastructure.Shared.Wrappers;
using MediatR;

namespace Blog.Service.Application.UseCases.CommentLike.Commands;

public class ToggleLikeCommand : IRequest<Response<bool>>
{
    public Guid Id { get; set; }
}
