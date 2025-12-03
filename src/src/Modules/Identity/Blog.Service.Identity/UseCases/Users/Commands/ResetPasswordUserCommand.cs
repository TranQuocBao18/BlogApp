using System;
using Blog.Infrastructure.Shared.Wrappers;
using MediatR;

namespace Blog.Service.Identity.UseCases.Users.Commands;

public partial class ResetPasswordUserCommand : IRequest<Response<string>>
{
    public Guid UserId { get; set; }
}
