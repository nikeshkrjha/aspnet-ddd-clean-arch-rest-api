using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Services.Authentication;
using ErrorOr;


namespace BuberDinner.Application.Services.Queries;

public interface IAuthenticationQueryService
{
    ErrorOr<AuthenticationResult> Login(string email, string password);
}