using System;
using Blog.Domain.Identity.Entities;
using Blog.Domain.Identity.Requests;
using Blog.Domain.Identity.Responses;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Identity.UseCases.Identity.Commands;
using Blog.Service.Identity.UseCases.Users.Commands;
using Blog.Shared.Auth;
using MediatR;
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
    private readonly ISecurityContextAccessor _securityContextAccessor;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    public AccountController(SignInManager<ApplicationUser> signInManager, ISecurityContextAccessor securityContextAccessor)
    {
        _signInManager = signInManager;
        _securityContextAccessor = securityContextAccessor;
    }

    [HttpPost("authenticate")]
    [ProducesResponseType(typeof(Response<AuthenticationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AuthenticateAsync(LoginUserCommand command)
    {
        command.IPAddress = GenerateIPAddress();
        var result = await Mediator.Send(command);
        if (!result.Succeeded)
        {
            return BadRequest(result);
        }
        Response.Cookies.Append(
            "REFRESH_TOKEN",
            result.Data.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            }
        );
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(Response<AuthenticationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenCommand command)
    {
        command.IPAddress = GenerateIPAddress();
        return Ok(await Mediator.Send(command));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync(LogoutCommand command)
    {
        if (User?.Identity?.IsAuthenticated == true)
        {
            var userId = _securityContextAccessor.UserId;
            command.UserId = userId;
            command.IPAddress = GenerateIPAddress();
            await _signInManager.SignOutAsync();

            Response.Cookies.Delete("REFRESH_TOKEN", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/"
            });
            return Ok(await Mediator.Send(command));
        }
        return BadRequest(new Response<bool>(false, "User is not authenticated"));
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
