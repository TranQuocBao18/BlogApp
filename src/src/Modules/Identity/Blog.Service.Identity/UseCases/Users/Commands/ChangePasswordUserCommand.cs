using System;
using Blog.Infrastructure.Shared.Wrappers;
using MediatR;

namespace Blog.Service.Identity.UseCases.Users.Commands;

public class ChangePasswordUserCommand : IRequest<Response<bool>>
{
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
}
