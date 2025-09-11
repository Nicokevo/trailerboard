using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrailerBoard.Application.Auth.Queries.GetCurrentUser;
using TrailerBoard.Contracts;
using TrailerBoard.Application.Auth.Commands.Login;
using TrailerBoard.Application.Auth.Commands.Register;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new LoginCommand(request.Email, request.Password), ct);
        return Ok(result);
    }

    [HttpGet("current")]
    [Authorize]
    public async Task<ActionResult<AuthResponse>> Current(CancellationToken ct)
    => Ok(await _mediator.Send(new GetCurrentUserQuery(User), ct));


    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new RegisterCommand(
            request.Email,
            request.Password,
            request.ConfirmPassword,
            request.FirstName,
            request.LastName
        ), ct);

        return Ok(result);
    }
}
