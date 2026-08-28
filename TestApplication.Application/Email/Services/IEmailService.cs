using TestApplication.Application.Email.Models;

namespace TestApplication.Application.Email.Services;

public interface IEmailService
{
    Task<SendEmailResult> SendEmailAsync(
        SendEmailRequest request,
        CancellationToken cancellationToken = default);
}
