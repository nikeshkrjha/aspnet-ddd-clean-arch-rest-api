using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Application.Services.Authentication.Commands;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;

namespace BuberDinner.Application.Services.Queries.Authentication;

public class AuthenticationQueryService : IAuthenticationQueryService
{
    private readonly IJwtTokenGenerator _iJwtTokenGenerator;
    private readonly IUserRepository _iUserRepository;

    public AuthenticationQueryService(IJwtTokenGenerator iJwtTokenGenerator, IUserRepository userRepository)
    {
        _iJwtTokenGenerator = iJwtTokenGenerator;
        _iUserRepository = userRepository;
    }

    public ErrorOr<AuthenticationResult> Login(string email, string password)
    {
        if (_iUserRepository.GetUserByEmail(email) is not User user)
        {
            return Errors.Authentication.InvalidCredentials;
        }
        if (user.Password != password)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var token = _iJwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(
            user, token
            );
    }
}