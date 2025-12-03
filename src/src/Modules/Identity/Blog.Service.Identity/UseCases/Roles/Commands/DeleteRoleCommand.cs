using System;
using Blog.Infrastructure.Shared.Wrappers;
using MediatR;

namespace Blog.Service.Identity.UseCases.Roles.Commands;

public partial class DeleteRoleCommand : IRequest<Response<Guid>>
{
    public Guid Id { get; set; }
}
