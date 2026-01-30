using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Identity.Responses;
using MediatR;

namespace Blog.Service.Identity.UseCases.Identity.Commands;

public class LoginUserCommand : IRequest<Response<AuthenticationResponse>>
{
    public string IPAddress { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}
