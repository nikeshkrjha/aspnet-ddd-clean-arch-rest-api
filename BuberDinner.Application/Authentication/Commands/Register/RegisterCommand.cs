using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuberDinner.Application.Services.Authentication;
using MediatR;
using ErrorOr;

namespace BuberDinner.Application.Authentication.Commands.Register
{
    public record RegisterCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password
    ) : IRequest<ErrorOr<AuthenticationResult>>;
}