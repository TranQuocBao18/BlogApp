using System;
using Blog.Model.Dto.Shared.Filters;

namespace Blog.Service.Communication.UseCases.NotificationMessage.Queries;

public class SearchListNotificationParameter : RequestParameter
{
    public string? Search { get; set; }
}
