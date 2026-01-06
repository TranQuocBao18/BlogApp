using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Communication.Response;
using MediatR;

namespace Blog.Service.Communication.UseCases.NotificationMessage.Queries;

public class GetNotificationByIdQuery : IRequest<Response<GetNotificationByIdResponse>>
{
    public Guid Id { get; set; } = Guid.Empty;
}
