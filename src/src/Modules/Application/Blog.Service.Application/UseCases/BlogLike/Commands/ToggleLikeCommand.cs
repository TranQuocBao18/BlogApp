using System;
using Blog.Infrastructure.Shared.Wrappers;
using MediatR;

namespace Blog.Service.Application.UseCases.BlogLike.Commands;

public class ToggleLikeCommand : IRequest<Response<bool>>
{
    public Guid Id { get; set; }
}
