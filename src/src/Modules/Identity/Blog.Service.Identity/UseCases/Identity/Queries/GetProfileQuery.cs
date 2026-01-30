using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Identity.Responses;
using MediatR;

namespace Blog.Service.Identity.UseCases.Identity.Queries;

public class GetProfileQuery : IRequest<Response<ProfileResponse>>
{
    public Guid Id { get; set; }
}
