using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Identity.Responses;
using Blog.Presentation.Shared.Controllers;
using Blog.Service.Identity.UseCases.Roles.Commands;
using Blog.Service.Identity.UseCases.Roles.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Presentation.Identity.Controllers.v1;

[ApiVersion("1.0")]
public class RoleController : BaseApiController
{
    // GET: api/<controller>
    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<RolesResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        return Ok(await Mediator.Send(new GetRolesQuery()));
    }

    // POST api/<controller>
    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post(UpsertRoleCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    // POST api/<controller>
    [HttpDelete("{RoleId}")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete([FromRoute] Guid roleId)
    {
        return Ok(await Mediator.Send(new DeleteRoleCommand
        {
            Id = roleId,
        }));
    }

    // PUT api/<controller>
    [HttpPut]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Put(UpsertRoleCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

}
