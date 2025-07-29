using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.DTO;
using MyWebApp.Interfaces;
using MyWebApp.Constants;

namespace MyWebApp.Extensions;

public static class EndPointsExtensions
{
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost(AuthRoutes.SignUp, async ([FromServices] IAuthenticationService authenticationService, SignUpModel model)
            => await authenticationService.RegisterUserAsync(model));

        app.MapPost(AuthRoutes.SignIn,
            async ([FromServices] IAuthenticationService authenticationService, SignInModel model)
                => await authenticationService.LoginUserAsync(model));
        
        return app;
    }

    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        app.MapGet(UserRoutes.Profile, async (IUserService userService)
                => await userService.GetUserProfileAsync())
            .RequireAuthorization();
        
        app.MapPut(UserRoutes.Update, async ([FromServices] IUserService userService, [FromBody] UserUpdateModel model) 
                    => await userService.UpdateUserProfileAsync(model))
            .RequireAuthorization();
        
        app.MapDelete(UserRoutes.Delete, async (IUserService userService) 
                => await userService.DeleteUserProfileAsync())
            .RequireAuthorization();

        return app;
    }
}