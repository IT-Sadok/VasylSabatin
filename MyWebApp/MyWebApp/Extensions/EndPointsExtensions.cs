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
        app.MapPost(WorkoutRoutes.Create, async (
                [FromServices] IWorkoutService workoutService,
                [FromBody] WorkoutModel model)
            => await workoutService.CreateWorkoutAsync(model))
            .RequireAuthorization();

        app.MapGet(WorkoutRoutes.GetAll, async (
                [FromServices] IWorkoutService workoutService)
            => await workoutService.GetAllWorkoutsAsync())
            .RequireAuthorization();

        app.MapPut(WorkoutRoutes.Update, async (
                [FromServices] IWorkoutService workoutService,
                [FromRoute] int id,
                [FromBody] WorkoutModel model)
            => await workoutService.UpdateWorkoutAsync(id, model))
            .RequireAuthorization();

        app.MapDelete(WorkoutRoutes.Delete, async (
                [FromServices] IWorkoutService workoutService,
                [FromRoute] int id)
            => await workoutService.DeleteWorkoutAsync(id))
            .RequireAuthorization();

        app.MapGet(WorkoutRoutes.Search, async (
                [FromServices] IWorkoutService workoutService,
                [FromQuery] string keyword)
            => await workoutService.SearchWorkoutsByKeywordAsync(keyword))
            .RequireAuthorization();

        app.MapPost(WorkoutRoutes.Sort, async (
                [FromServices] IWorkoutService workoutService,
                [FromBody] WorkoutSortByDateModel model)
            => await workoutService.SortWorkoutsByDateAsync(model))
            .RequireAuthorization();

        return app;
    }
}