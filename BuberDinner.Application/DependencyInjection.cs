using BuberDinner.Application.Services.Authentication;
using BuberDinner.Application.Services.Authentication.Commands;
using BuberDinner.Application.Services.Commands.Authentication;
using BuberDinner.Application.Services.Queries;
using BuberDinner.Application.Services.Queries.Authentication;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace BuberDinner.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(typeof(DependencyInjection).Assembly);
        return services;
    }
}