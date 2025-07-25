using Microsoft.AspNetCore.Mvc;
using MyWebApp.DTO;
using MyWebApp.Interfaces;
using MyWebApp.Constants;

namespace MyWebApp.Extensions;

public static class EndPointsExtensions
{
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost(AuthConstants.SignUp, async ([FromServices] IAuthenticationService authenticationService, SignUpModel model)
            => await authenticationService.RegisterUserAsync(model));
        
        app.MapPost(AuthConstants.SignIn, async ([FromServices] IAuthenticationService authenticationService, SignInModel model)
            => await authenticationService.LoginUserAsync(model));
        
        return app;
    }
}