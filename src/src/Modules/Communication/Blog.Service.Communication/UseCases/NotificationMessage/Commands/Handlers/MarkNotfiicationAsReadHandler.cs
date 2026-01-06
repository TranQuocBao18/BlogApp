using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Communication.Response;
using Blog.Service.Communication.Interfaces;
using MediatR;

namespace Blog.Service.Communication.UseCases.NotificationMessage.Commands.Handlers;

public class MarkNotfiicationAsReadHandler : IRequestHandler<MarkNotificationAsReadCommand, Response<MarkNotificationAsReadResponse>>
{
    private readonly INotificationService _service;

    public MarkNotfiicationAsReadHandler(INotificationService service)
    {
        _service = service;
    }

    public async Task<Response<MarkNotificationAsReadResponse>> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        var response = await _service.MarkNotificationAsRead(request.Payload, cancellationToken);

        return response;
    }
}
