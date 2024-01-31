using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
    {

        private readonly IJwtTokenGenerator _iJwtTokenGenerator;
        private readonly IUserRepository _iUserRepository;

        public LoginQueryHandler(IJwtTokenGenerator tokenGenerator, IUserRepository userRepository)
        {
            _iJwtTokenGenerator = tokenGenerator;
            _iUserRepository = userRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
        {

            if (_iUserRepository.GetUserByEmail(query.Email) is not User user)
            {
                return Errors.Authentication.InvalidCredentials;
            }
            if (user.Password != query.Password)
            {
                return Errors.Authentication.InvalidCredentials;
            }

            var token = _iJwtTokenGenerator.GenerateToken(user);
            return new AuthenticationResult(
                user, token
            );
        }
    }
}