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
        app.MapGet(UserRoutes.Profile, async (IUserService userService, ClaimsPrincipal user)
                => await userService.GetUserProfileAsync(user))
            .RequireAuthorization();
        
        app.MapPatch(UserRoutes.Update,
                async ([FromServices] IUserService userService,
                        [FromBody] UserUpdateModel dto,
                        ClaimsPrincipal user) =>
                    await userService.UpdateUserProfileAsync(dto, user))
            .RequireAuthorization();
        
        app.MapDelete(UserRoutes.Delete, async (HttpContext context, IUserService userService) =>
            {
                var userId = context.GetUserId();
                await userService.DeleteUserProfileAsync(userId);
            })
            .RequireAuthorization();

        return app;
    }
}