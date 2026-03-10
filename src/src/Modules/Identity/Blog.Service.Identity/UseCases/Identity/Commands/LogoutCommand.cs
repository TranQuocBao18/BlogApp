using System;
using Blog.Infrastructure.Shared.Wrappers;
using MediatR;

namespace Blog.Service.Identity.UseCases.Identity.Commands;

public class LogoutCommand : IRequest<Response<bool>>
{
    public Guid UserId { get; set; }
    public string IPAddress { get; set; } = string.Empty;
}
