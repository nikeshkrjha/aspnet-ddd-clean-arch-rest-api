using BuberDinner.Api.Errors;
using BuberDinner.Api;
using BuberDinner.Application;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentation()
        .AddApplication()
        .AddInfrastructure(builder.Configuration);
}

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();
{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    // app.UseAuthorization();
    app.MapControllers();
    app.Run();
}

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }


