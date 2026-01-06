using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Communication.Response;
using MediatR;

namespace Blog.Service.Communication.UseCases.NotificationMessage.Queries;

public class GetListNotificationByUserIdQuery : IRequest<PagedResponse<IReadOnlyList<GetListNotificationByUserResponse>>>
{
    public string UserId { get; set; } = string.Empty;
    public string? Search { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
