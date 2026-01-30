using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Identity.Responses;
using MediatR;

namespace Blog.Service.Identity.UseCases.Users.Queries;

public class GetUserByIdQuery : IRequest<Response<UserResponse>>
{
    public Guid Id { get; set; }
}
