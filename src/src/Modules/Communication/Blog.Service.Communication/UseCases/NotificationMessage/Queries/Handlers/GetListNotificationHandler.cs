using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Communication.Response;
using Blog.Service.Communication.Interfaces;
using MediatR;

namespace Blog.Service.Communication.UseCases.NotificationMessage.Queries.Handlers;

public sealed class GetListNotificationHandler : IRequestHandler<GetListNotificationQuery, PagedResponse<IReadOnlyList<GetListNotificationResponse>>>
{
    private readonly INotificationService _service;

    public GetListNotificationHandler(INotificationService service)
    {
        _service = service;
    }

    public async Task<PagedResponse<IReadOnlyList<GetListNotificationResponse>>> Handle(GetListNotificationQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetListNotification(request.PageNumber, request.PageSize, request.Search, cancellationToken);
    }
}
