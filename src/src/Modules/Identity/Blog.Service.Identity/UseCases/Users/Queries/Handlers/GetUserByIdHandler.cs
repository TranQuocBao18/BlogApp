using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Identity.Responses;
using Blog.Service.Identity.Interfaces;
using MediatR;

namespace Blog.Service.Identity.UseCases.Users.Queries.Handlers;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Response<UserResponse>>
{
    private readonly IUserService _service;

    public GetUserByIdHandler(IUserService service)
    {
        _service = service;
    }

    public async Task<Response<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = request.Id;
        return await _service.GetUserByIdAsync(userId, cancellationToken);
    }
}
