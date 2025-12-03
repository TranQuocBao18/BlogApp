using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Identity.Requests;
using MediatR;

namespace Blog.Service.Identity.UseCases.Users.Commands;

public partial class UpsertUserCommand : IRequest<Response<Guid>>
{
    public UserRequest? Payload { get; set; }
}
