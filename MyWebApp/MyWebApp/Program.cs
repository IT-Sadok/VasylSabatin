using MyWebApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.DTO;
using MyWebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProjectServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("sign-up", async ([FromServices] IAuthenticationService authenticationService, SignUpModel model)
    => await authenticationService.RegisterUserAsync(model));

app.MapPost("sign-in", async ([FromServices] IAuthenticationService authenticationService, SignInModel model)
    => await authenticationService.LoginUserAsync(model));

app.Run();