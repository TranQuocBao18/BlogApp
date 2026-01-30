using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Communication.Response;
using Blog.Service.Communication.Interfaces;
using Blog.Utilities.Extensions;
using MediatR;

namespace Blog.Service.Communication.UseCases.NotificationMessage.Queries.Handlers;

public class GetListNotificationByUserHandler : IRequestHandler<GetListNotificationByUserIdQuery, PagedResponse<IReadOnlyList<GetListNotificationByUserResponse>>>
{
    private readonly INotificationService _service;

    public GetListNotificationByUserHandler(INotificationService service)
    {
        _service = service;
    }

    public async Task<PagedResponse<IReadOnlyList<GetListNotificationByUserResponse>>> Handle(GetListNotificationByUserIdQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetListNotificationByUserId(request.UserId.AsGuid(), request.PageNumber, request.PageSize, cancellationToken);
    }
}
