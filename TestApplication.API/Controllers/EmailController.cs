using MediatR;
using Microsoft.AspNetCore.Mvc;
using TestApplication.Application.Email.Commands;
using TestApplication.Application.Email.Models;

namespace TestApplication.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmailController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendEmail(
        [FromBody] SendEmailRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new SendEmailCommand(request),
            cancellationToken);

        return result.Success
            ? Ok(result)
            : BadRequest(result);
    }
}
