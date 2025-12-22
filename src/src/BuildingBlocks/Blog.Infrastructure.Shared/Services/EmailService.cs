using System;
using Blog.Domain.Shared.Settings;
using Blog.Infrastructure.Shared.Exceptions;
using Blog.Infrastructure.Shared.Interfaces;
using Blog.Model.Dto.Shared.Outbox;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Blog.Infrastructure.Shared.Services;

public class EmailService : IEmailService
{
    public MailSettings _mailSettings { get; }
    public ILogger<EmailService> _logger { get; }

    public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
    {
        _mailSettings = mailSettings.Value;
        _logger = logger;
    }

    public async Task SendAsync(EmailRequest request)
    {
        try
        {
            // create message
            var email = new MimeMessage();
            var fromAddress = request.From ?? _mailSettings.EmailFrom;
            if (string.IsNullOrWhiteSpace(fromAddress))
            {
                throw new ArgumentException("From address cannot be null or empty.");
            }

            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, fromAddress));
            email.Sender = MailboxAddress.Parse(fromAddress);

            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = request.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (sender, certificate, certChainType, errors) => true;
            smtp.AuthenticationMechanisms.Remove("XOAUTH2");
            smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);

        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw new ApiException(ex.Message);
        }
    }
}
