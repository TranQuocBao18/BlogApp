using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Identity.Requests;
using MediatR;

namespace Blog.Service.Identity.UseCases.Roles.Commands;

public partial class UpsertRoleCommand : IRequest<Response<Guid>>
{
    public RoleRequest? Payload { get; set; }
}
