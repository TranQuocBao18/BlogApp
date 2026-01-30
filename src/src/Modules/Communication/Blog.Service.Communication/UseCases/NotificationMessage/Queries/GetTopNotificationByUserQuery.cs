using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Communication.Response;
using MediatR;

namespace Blog.Service.Communication.UseCases.NotificationMessage.Queries;

public class GetTopNotificationByUserQuery : IRequest<Response<GetTopNotificationUnreadResponse>>
{

}
