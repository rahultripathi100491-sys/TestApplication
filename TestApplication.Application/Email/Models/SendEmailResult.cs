namespace TestApplication.Application.Email.Models;

public sealed class SendEmailResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
}
