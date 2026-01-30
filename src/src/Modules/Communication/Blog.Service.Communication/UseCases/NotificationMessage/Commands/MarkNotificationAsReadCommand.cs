using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Communication.Requests;
using Blog.Domain.Communication.Response;
using MediatR;

namespace Blog.Service.Communication.UseCases.NotificationMessage.Commands;

public class MarkNotificationAsReadCommand : IRequest<Response<MarkNotificationAsReadResponse>>
{
    public MarkNotificationAsReadRequest Payload { get; set; } = default!;
}
