using System;
using Blog.Model.Dto.Shared.Outbox;

namespace Blog.Infrastructure.Shared.Interfaces;

public interface IEmailService
{
    Task SendAsync(EmailRequest request);
}
