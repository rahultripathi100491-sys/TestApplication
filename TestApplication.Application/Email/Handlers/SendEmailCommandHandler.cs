using MediatR;
using TestApplication.Application.Email.Models;
using TestApplication.Application.Email.Services;

namespace TestApplication.Application.Email.Handlers;

public sealed class SendEmailCommandHandler
    : IRequestHandler<SendEmailCommand, SendEmailResult>
{
    private readonly IEmailService _emailService;

    public SendEmailCommandHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public Task<SendEmailResult> Handle(
        SendEmailCommand request,
        CancellationToken cancellationToken)
    {
        return _emailService.SendEmailAsync(
            request.Request,
            cancellationToken);
    }
}
