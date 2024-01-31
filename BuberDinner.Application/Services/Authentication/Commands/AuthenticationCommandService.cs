using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Application.Services.Authentication.Commands;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;

namespace BuberDinner.Application.Services.Commands.Authentication;

public class AuthenticationCommandService : IAuthenticationCommandService
{
    private readonly IJwtTokenGenerator _iJwtTokenGenerator;
    private readonly IUserRepository _iUserRepository;

    public AuthenticationCommandService(IJwtTokenGenerator iJwtTokenGenerator, IUserRepository userRepository)
    {
        _iJwtTokenGenerator = iJwtTokenGenerator;
        _iUserRepository = userRepository;
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