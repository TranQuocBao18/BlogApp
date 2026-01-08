using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Responses;
using Blog.Service.Application.UseCases.Comment.Commands;
using Blog.Service.Application.UseCases.Comment.Queries;
using Blog.Shared.Auth;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Presentation.Application.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiVersion("1.0")]
public class CommentController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public CommentController(ISecurityContextAccessor securityContextAccessor)
    {
        _securityContextAccessor = securityContextAccessor;
    }

    // GET: api/v1/<controller>
    [HttpGet("blogId:guid")]
    [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<CommentResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] GetCommentsParameter filter, Guid blogId)
    {
        return Ok(await Mediator.Send(new GetListCommentByBlogIdQuery() { Id = blogId, PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
    }

    // GET: api/v1/<controller>
    [HttpGet("parentId:guid")]
    [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<CommentResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByParentId([FromQuery] GetCommentsParameter filter, Guid parentId)
    {
        return Ok(await Mediator.Send(new GetRepliesByParentIdQuery() { Id = parentId, PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
    }

    // POST: api/v1/<controller>
    [HttpPost]
    [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post(UpsertCommentCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    // PUT: api/v1/<controller>
    [HttpPut]
    [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Put(UpsertCommentCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    // DELETE: api/v1/<controller>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await Mediator.Send(new DeleteCommentByIdCommand { Id = id }));
    }
}
