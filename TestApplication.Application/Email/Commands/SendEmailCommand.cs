using MediatR;
using TestApplication.Application.Email.Models;

namespace TestApplication.Application.Email.Commands;

public sealed record SendEmailCommand(SendEmailRequest Request)
    : IRequest<SendEmailResult>;
