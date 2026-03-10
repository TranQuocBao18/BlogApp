using System;
using Blog.Domain.Identity.Responses;
using Blog.Infrastructure.Shared.Wrappers;
using MediatR;

namespace Blog.Service.Identity.UseCases.Identity.Commands;

public class RefreshTokenCommand : IRequest<Response<AuthenticationResponse>>
{
    public string IPAddress { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
