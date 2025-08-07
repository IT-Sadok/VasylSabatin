using Microsoft.AspNetCore.Mvc;
using MyWebApp.DTO;
using MyWebApp.Interfaces;
using MyWebApp.Constants;
using MyWebApp.Models;
using MyWebApp.Services.Interfaces;

namespace MyWebApp.Extensions;

public static class EndPointsExtensions
{
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost(AuthRoutes.SignUp, async (
                [FromServices] IAuthenticationService authenticationService,
                [FromBody] SignUpModel model)
            => await authenticationService.RegisterUserAsync(model));

        app.MapPost(AuthRoutes.SignIn, async (
                [FromServices] IAuthenticationService authenticationService,
                [FromBody] SignInModel model)
            => await authenticationService.LoginUserAsync(model));

        return app;
    }

    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        app.MapGet(UserRoutes.Profile, async (
                [FromServices] IUserService userService)
            => await userService.GetUserProfileAsync())
            .RequireAuthorization();

        app.MapPut(UserRoutes.Update, async (
                [FromServices] IUserService userService,
                [FromBody] UserUpdateModel model)
            => await userService.UpdateUserProfileAsync(model))
            .RequireAuthorization();

        app.MapDelete(UserRoutes.Delete, async (
                [FromServices] IUserService userService)
            => await userService.DeleteUserProfileAsync())
            .RequireAuthorization();

        return app;
    }

    public static WebApplication MapWorkoutEndpoints(this WebApplication app)
    {
        app.MapPost(WorkoutRoutes.Base, async (
                [FromServices] IWorkoutService workoutService,
                [FromBody] WorkoutModel model,
                    CancellationToken token)
            => await workoutService.CreateWorkoutAsync(model, token))
            .RequireAuthorization();

        app.MapGet(WorkoutRoutes.Base, async (
                [FromServices] IWorkoutService workoutService,
                CancellationToken token)
            => await workoutService.GetAllWorkoutsAsync(token))
            .RequireAuthorization();

        app.MapPut(WorkoutRoutes.ById, async (
                [FromServices] IWorkoutService workoutService,
                [FromRoute] int id,
                [FromBody] WorkoutModel model,
                CancellationToken token)
            => await workoutService.UpdateWorkoutAsync(id, model, token))
            .RequireAuthorization();

        app.MapDelete(WorkoutRoutes.ById, async (
                [FromServices] IWorkoutService workoutService,
                [FromRoute] int id,
                CancellationToken token)
            => await workoutService.DeleteWorkoutAsync(id, token))
            .RequireAuthorization();

        app.MapGet(WorkoutRoutes.Base, async (
                [FromServices] IWorkoutService workoutService,
                [FromQuery] string keyword,
                CancellationToken token) => 
            !string.IsNullOrWhiteSpace(keyword) 
                ? await workoutService.SearchWorkoutsByKeywordAsync(keyword, token)
                : await workoutService.GetAllWorkoutsAsync(token))
                .RequireAuthorization();

        app.MapPost(WorkoutRoutes.Sort, async (
                [FromServices] IWorkoutService workoutService,
                [FromBody] WorkoutSortByDateModel model,
                CancellationToken token)
            => await workoutService.SortWorkoutsByDateAsync(model, token))
            .RequireAuthorization();

        return app;
    }
}