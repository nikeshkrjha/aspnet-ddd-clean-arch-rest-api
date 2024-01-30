using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _iJwtTokenGenerator;
    private readonly IUserRepository _iUserRepository;

    public AuthenticationService(IJwtTokenGenerator iJwtTokenGenerator, IUserRepository userRepository)
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

    public ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
    {
        // Check if the user doesn't exist
        if (_iUserRepository.GetUserByEmail(email) is not null)
        {
            // throw new Exception("User with given email already exists");

            return Errors.User.DuplicateEmail;
        }

        // Add user to db
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };
        _iUserRepository.Add(user);

        // Create JWT Token
        var token = _iJwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult
        (
            user,
            token
        );
    }
}