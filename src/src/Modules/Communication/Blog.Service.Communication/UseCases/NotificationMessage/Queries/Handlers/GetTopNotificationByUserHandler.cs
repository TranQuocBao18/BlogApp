using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Communication.Response;
using Blog.Service.Communication.Interfaces;
using MediatR;

namespace Blog.Service.Communication.UseCases.NotificationMessage.Queries.Handlers;

public class GetTopNotificationByUserHandler : IRequestHandler<GetTopNotificationByUserQuery, Response<GetTopNotificationUnreadResponse>>
{
    private readonly INotificationService _service;

    public GetTopNotificationByUserHandler(INotificationService service)
    {
        _service = service;
    }

    public async Task<Response<GetTopNotificationUnreadResponse>> Handle(GetTopNotificationByUserQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetTopNotificationUnreadByUser(cancellationToken);
    }
}
