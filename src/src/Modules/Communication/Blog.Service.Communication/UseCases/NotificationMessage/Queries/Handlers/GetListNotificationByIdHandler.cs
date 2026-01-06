using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Communication.Response;
using Blog.Service.Communication.Interfaces;
using MediatR;

namespace Blog.Service.Communication.UseCases.NotificationMessage.Queries.Handlers;

public class GetListNotificationByIdHandler : IRequestHandler<GetNotificationByIdQuery, Response<GetNotificationByIdResponse>>
{
    private readonly INotificationService _service;

    public GetListNotificationByIdHandler(INotificationService service)
    {
        _service = service;
    }

    public async Task<Response<GetNotificationByIdResponse>> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetNotificationById(request.Id, cancellationToken);
    }
}
