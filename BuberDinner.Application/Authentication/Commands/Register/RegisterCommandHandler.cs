using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Domain.Entities;
using BuberDinner.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _iJwtTokenGenerator;
        private readonly IUserRepository _iUserRepository;

        public RegisterCommandHandler(IJwtTokenGenerator tokenGenerator, IUserRepository userRepository)
        {
            _iJwtTokenGenerator = tokenGenerator;
            _iUserRepository = userRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (_iUserRepository.GetUserByEmail(request.Email) is not null)
            {
                return Errors.User.DuplicateEmail;
            }

            // Add user to db
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password
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
}