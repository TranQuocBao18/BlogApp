using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Application.Responses;
using Blog.Service.Application.UseCases.Tag.Commands;
using Blog.Service.Application.UseCases.Tag.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Presentation.Application.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiVersion("1.0")]
public class TagController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    public TagController()
    {

    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<TagResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] GetTagsParameter filter)
    {
        return Ok(await Mediator.Send(new GetTagsQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
    }

    // POST: api/v1/<controller>/search
    [HttpPost("search")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<TagResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post(SearchTagsParameter search)
    {
        return Ok(await Mediator.Send(new GetTagsQuery() { PageSize = search.PageSize, PageNumber = search.PageNumber, Search = search.Search }));
    }

    // POST: api/v1/<controller>
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post(UpsertTagCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    // PUT: api/v1/<controller>
    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Put(UpsertTagCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    // DELETE: api/v1/<controller>
    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await Mediator.Send(new DeleteTagByIdCommand { Id = id }));
    }
}
