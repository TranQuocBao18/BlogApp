using System;
using Blog.Infrastructure.Shared.Wrappers;
using MediatR;

namespace Blog.Service.Identity.UseCases.Users.Commands;

public partial class DeleteUserByIdCommand : IRequest<Response<bool>>
{
    public Guid Id { get; set; }
}
