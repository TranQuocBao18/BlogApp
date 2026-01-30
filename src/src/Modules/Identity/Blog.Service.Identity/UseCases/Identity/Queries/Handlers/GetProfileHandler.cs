using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Identity.Responses;
using Blog.Service.Identity.Interfaces;
using MediatR;

namespace Blog.Service.Identity.UseCases.Identity.Queries.Handlers;

public class GetProfileHandler : IRequestHandler<GetProfileQuery, Response<ProfileResponse>>
{
    private readonly IAccountService _accountService;

    public GetProfileHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<Response<ProfileResponse>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = request.Id;
        return await _accountService.GetProfileAsync(userId);
    }
}
