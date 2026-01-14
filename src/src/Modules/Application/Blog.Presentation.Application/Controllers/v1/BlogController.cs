using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Responses;
using Blog.Service.Application.UseCases.Blog.Commands;
using Blog.Service.Application.UseCases.Blog.Queries;
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
// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiVersion("1.0")]
public class BlogController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public BlogController(ISecurityContextAccessor securityContextAccessor)
    {
        _securityContextAccessor = securityContextAccessor;
    }

    // GET: api/v1/<controller>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<BlogResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] GetBlogsParameter filter)
    {
        return Ok(await Mediator.Send(new GetBlogsPublishedQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
    }

    // GET: api/v1/<controller>/categoryId
    [HttpGet("{categoryId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<BlogResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCategoryId([FromQuery] GetBlogsParameter filter, Guid categoryId)
    {
        return Ok(await Mediator.Send(new GetBlogsPublishedByCategoryIdQuery() { Id = categoryId, PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
    }

    // GET: api/v1/<controller>/admin
    [HttpGet("admin")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<BlogResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetBlogsParameter filter)
    {
        return Ok(await Mediator.Send(new GetBlogsQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
    }

    // GET: api/v1/<controller>/slug
    [HttpGet("{slug}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BlogResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBySlug([FromRoute] string slug)
    {
        return Ok(await Mediator.Send(new GetBlogBySlugQuery() { Slug = slug }));
    }

    // POST: api/v1/<controller>/search
    [HttpPost("search")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<BlogResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post(SearchBlogsParameter search)
    {
        return Ok(await Mediator.Send(new GetBlogsPublishedQuery() { PageSize = search.PageSize, PageNumber = search.PageNumber, Search = search.Search }));
    }

    // POST: api/v1/<controller>/admin/search
    [HttpPost("admin/search")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<BlogResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchBlogs(SearchBlogsParameter search)
    {
        return Ok(await Mediator.Send(new GetBlogsQuery() { PageSize = search.PageSize, PageNumber = search.PageNumber, Search = search.Search }));
    }

    // POST: api/v1/<controller>/categoryId/search
    [HttpPost("{categoryId}/search")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<BlogResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post(Guid categoryId, SearchBlogsParameter search)
    {
        return Ok(await Mediator.Send(new GetBlogsPublishedByCategoryIdQuery() { Id = categoryId, PageSize = search.PageSize, PageNumber = search.PageNumber, Search = search.Search }));
    }

    // POST: api/v1/<controller>
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post(UpsertBlogCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    // PUT: api/v1/<controller>
    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Put(UpsertBlogCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    // DELETE: api/v1/<controller>
    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await Mediator.Send(new DeleteBlogByIdCommand { Id = id }));
    }
}
