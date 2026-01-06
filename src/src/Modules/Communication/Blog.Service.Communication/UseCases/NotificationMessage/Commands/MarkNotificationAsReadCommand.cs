using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Communication.Requests;
using Blog.Model.Dto.Communication.Response;
using MediatR;

namespace Blog.Service.Communication.UseCases.NotificationMessage.Commands;

public class MarkNotificationAsReadCommand : IRequest<Response<MarkNotificationAsReadResponse>>
{
    public MarkNotificationAsReadRequest Payload { get; set; } = default!;
}
