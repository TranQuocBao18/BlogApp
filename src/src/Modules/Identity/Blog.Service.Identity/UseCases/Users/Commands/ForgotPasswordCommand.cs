using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Identity.Requests;
using MediatR;

namespace Blog.Service.Identity.UseCases.Users.Commands;

public partial class ForgotPasswordCommand : IRequest<Response<bool>>
{
    public ForgotPasswordRequest Payload { get; set; }
    public string IPAddress { get; set; }
}
