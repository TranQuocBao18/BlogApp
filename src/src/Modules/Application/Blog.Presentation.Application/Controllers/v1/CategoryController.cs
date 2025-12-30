using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Responses;
using Blog.Service.Application.UseCases.Category.Commands;
using Blog.Service.Application.UseCases.Category.Queries;
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
public class CategoryController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    public CategoryController()
    {

    }

    // GET: api/v1/<controller>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<CategoryResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] GetCategoriesParameter filter)
    {
        return Ok(await Mediator.Send(new GetCategoriesQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
    }

    // POST: api/v1/<controller>/search
    [HttpPost("search")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<CategoryResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post(SearchCategoriesParameter search)
    {
        return Ok(await Mediator.Send(new GetCategoriesQuery() { PageSize = search.PageSize, PageNumber = search.PageNumber, Search = search.Search }));
    }

    // POST: api/v1/<controller>
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post(UpsertCategoryCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    // PUT: api/v1/<controller>
    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Put(UpsertCategoryCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    // DELETE: api/v1/<controller>
    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await Mediator.Send(new DeleteCategoryByIdCommand { Id = id }));
    }
}
