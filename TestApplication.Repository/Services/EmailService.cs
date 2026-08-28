using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using TestApplication.Application.Email.Models;
using TestApplication.Application.Email.Services;

namespace TestApplication.Repository.Services;

public sealed class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<SendEmailResult> SendEmailAsync(
        SendEmailRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.To))
            return new SendEmailResult
            {
                Success = false,
                Message = "Recipient email is required."
            };

        if (string.IsNullOrWhiteSpace(request.Subject))
            return new SendEmailResult
            {
                Success = false,
                Message = "Subject is required."
            };

        var host = _configuration["EmailSettings:Host"];
        var portText = _configuration["EmailSettings:Port"];
        var username = _configuration["EmailSettings:Username"];
        var password = _configuration["EmailSettings:Password"];
        var from = _configuration["EmailSettings:From"];
        var fromName = _configuration["EmailSettings:FromName"] ?? "My Application";

        if (string.IsNullOrWhiteSpace(host) ||
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(from))
        {
            return new SendEmailResult
            {
                Success = false,
                Message = "EmailSettings are not configured correctly."
            };
        }

        if (!int.TryParse(portText, out var port))
            port = 587;

        if (!MailboxAddress.TryParse(from, out var fromAddress))
        {
            return new SendEmailResult
            {
                Success = false,
                Message = "EmailSettings:From is not a valid email address."
            };
        }

        if (!MailboxAddress.TryParse(request.To, out var toAddress))
        {
            return new SendEmailResult
            {
                Success = false,
                Message = "Recipient email address is not valid."
            };
        }

        var email = new MimeMessage();

        email.From.Add(new MailboxAddress(fromName, fromAddress.Address));
        email.To.Add(toAddress);
        email.Subject = request.Subject;

        email.Body = new TextPart("html")
        {
            Text = request.Body ?? string.Empty
        };

        using var smtp = new SmtpClient();

        try
        {
            await smtp.ConnectAsync(
                host,
                port,
                SecureSocketOptions.StartTls,
                cancellationToken);

            await smtp.AuthenticateAsync(
                username,
                password,
                cancellationToken);

            // If this completes without throwing, Gmail/SMTP accepted
            // the message for delivery.
            await smtp.SendAsync(email, cancellationToken);

            await smtp.DisconnectAsync(true, cancellationToken);

            return new SendEmailResult
            {
                Success = true,
                Message = "Email accepted by the SMTP server."
            };
        }
        catch (Exception ex)
        {
            if (smtp.IsConnected)
            {
                await smtp.DisconnectAsync(true, CancellationToken.None);
            }

            return new SendEmailResult
            {
                Success = false,
                Message = $"Email sending failed: {ex.Message}"
            };
        }
    }
}
