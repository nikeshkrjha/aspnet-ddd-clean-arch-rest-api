using Microsoft.AspNetCore.Mvc;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Application.Common.Errors;
using System.Net;
using ErrorOr;
using MediatR;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Queries.Login;
using MapsterMapper;


namespace BuberDinner.Api.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;
    public AuthenticationController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        ErrorOr<AuthenticationResult> authResult = await _mediator.Send(command);

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors)
        );
    }

    private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
        authResult.user.Id,
        authResult.user.FirstName,
        authResult.user.LastName,
        authResult.user.Email,
        authResult.Token);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var loginQuery = _mapper.Map<LoginQuery>(request);
        ErrorOr<AuthenticationResult> authResult = await _mediator.Send(loginQuery);

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors)
        );
    }
}