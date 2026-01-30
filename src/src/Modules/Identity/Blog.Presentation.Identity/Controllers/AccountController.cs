using System;
using Blog.Domain.Identity.Entities;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Identity.Requests;
using Blog.Domain.Identity.Responses;
using Blog.Service.Identity.UseCases.Identity.Commands;
using Blog.Service.Identity.UseCases.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Presentation.Identity.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private IMediator _mediator;
    private readonly SignInManager<ApplicationUser> _signInManager;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    public AccountController(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpPost("authenticate")]
    [ProducesResponseType(typeof(Response<AuthenticationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AuthenticateAsync(LoginUserCommand command)
    {
        command.IPAddress = GenerateIPAddress();
        return Ok(await Mediator.Send(command));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        if (User?.Identity?.IsAuthenticated == true)
        {
            await _signInManager.SignOutAsync();
            return SignOut(new AuthenticationProperties { RedirectUri = "/login" });
        }
        return Ok();
    }

    [HttpPost("forgotpassword")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var ipClient = GenerateIPAddress();
        return Ok(await Mediator.Send(new ForgotPasswordCommand { Payload = request, IPAddress = ipClient }));
    }

    [HttpPost]
    [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post(UpsertUserCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    private string GenerateIPAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            return Request.Headers["X-Forwarded-For"];
        }
        else
        {
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
