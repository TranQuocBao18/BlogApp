using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Identity.Responses;
using Blog.Presentation.Shared.Controllers;
using Blog.Service.Identity.UseCases.Identity.Queries;
using Blog.Service.Identity.UseCases.Roles.Queries;
using Blog.Service.Identity.UseCases.Users.Commands;
using Blog.Service.Identity.UseCases.Users.Queries;
using Blog.Shared.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Presentation.Identity.Controllers.v1;

[ApiVersion("1.0")]
public class UserController : BaseApiController
{
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public UserController(ISecurityContextAccessor securityContextAccessor)
    {
        _securityContextAccessor = securityContextAccessor;
    }


    // GET api/<controller>/profile
    [HttpGet("profile")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(Response<ProfileResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ProfileAsync()
    {
        var userId = _securityContextAccessor.UserId;
        return Ok(await Mediator.Send(new GetProfileQuery() { Id = userId }));
    }

    // GET api/<controller>/roles
    [HttpGet("roles")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> RolesAsync()
    {
        var userId = _securityContextAccessor.UserId;
        return Ok(await Mediator.Send(new GetRolesByUserIdQuery() { UserId = userId }));
    }

    // GET: api/<controller>
    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<UsersResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] GetUsersParameter filter)
    {
        return Ok(await Mediator.Send(new GetUsersQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
    }

    // POST api/<controller>/search
    [HttpPost("search")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<UsersResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post(SearchUsersParameter search)
    {
        return Ok(await Mediator.Send(new GetUsersQuery() { PageSize = search.PageSize, PageNumber = search.PageNumber, Search = search.Search }));
    }

    // GET api/<controller>/<id>
    [HttpGet("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(Response<UserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok(await Mediator.Send(new GetUserByIdQuery { Id = id }));
    }

    // POST api/<controller>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post(UpsertUserCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    // PUT api/<controller>/<id>
    [HttpPut()]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Put(UpsertUserCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    // DELETE api/<controller>/<id>
    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await Mediator.Send(new DeleteUserByIdCommand { Id = id }));
    }

    // POST api/<controller>
    [HttpPost("resetpassword")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordUserCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    // POST api/<controller>
    [HttpPost("changepassword")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordUserCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}
