using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Application.Requests;
using MediatR;

namespace Blog.Service.Application.UseCases.Tag.Commands;

public partial class UpsertTagCommand : IRequest<Response<Guid>>
{
    public TagRequest? Payload { get; set; }
}
