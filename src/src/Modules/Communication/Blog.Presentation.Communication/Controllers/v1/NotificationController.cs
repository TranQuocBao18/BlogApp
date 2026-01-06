using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Communication.Response;
using Blog.Service.Communication.UseCases.NotificationMessage.Commands;
using Blog.Service.Communication.UseCases.NotificationMessage.Queries;
using Blog.Shared.Auth;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Presentation.Communication.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiVersion("1.0")]
public class NotificationController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public NotificationController(ISecurityContextAccessor securityContextAccessor)
    {
        _securityContextAccessor = securityContextAccessor;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<GetListNotificationResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListNotification([FromQuery] GetListNotificationParameter query, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new GetListNotificationQuery
        {
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        }, cancellationToken);
        return Ok(response);
    }

    [HttpPost("search")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<GetListNotificationResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchListNotification([FromQuery] SearchListNotificationParameter query, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new GetListNotificationQuery
        {
            Search = query.Search,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        }, cancellationToken);
        return Ok(response);
    }

    [HttpGet("users/{userId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<CreateUserNotificationResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListNotificationByUser([FromRoute] Guid userId, [FromQuery] GetListNotificationParameter parameter, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new GetListNotificationByUserIdQuery
        {
            UserId = userId.ToString(),
            PageNumber = parameter.PageNumber,
            PageSize = parameter.PageSize
        }, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{notificationId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(Response<GetNotificationByIdResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotificationById([FromRoute] GetNotificationByIdQuery query, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    [HttpGet("top")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(Response<GetTopNotificationUnreadResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopNotificationByUser(CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new GetTopNotificationByUserQuery { }, cancellationToken);
        return Ok(response);
    }

    [HttpPost("mark-as-read")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(Response<MarkNotificationAsReadResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> MarkNotificationAsRead([FromBody] MarkNotificationAsReadCommand command, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(command, cancellationToken);
        return Ok(response);
    }


}
