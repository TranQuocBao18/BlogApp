using System;
using Blog.Infrastructure.Shared.Wrappers;
using MediatR;

namespace Blog.Service.Identity.UseCases.Identity.Commands;

public class RegisterUserCommand : IRequest<Response<string>>
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string Origin { get; set; }
}
