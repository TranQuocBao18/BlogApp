using System;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Presentation.Shared.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
public abstract class BaseApiController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
}
