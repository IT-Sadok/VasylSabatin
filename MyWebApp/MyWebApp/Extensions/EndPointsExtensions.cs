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
        var group = app.MapGroup(AuthRoutes.Base)
            .WithTags("Authentication")
            .AllowAnonymous();  
        
        group.MapPost(AuthRoutes.SignUp, async (IAuthenticationService authenticationService,
                    SignUpModel model) 
                => await authenticationService.RegisterUserAsync(model))
            .WithName("RegisterUser")
            .WithSummary("Register user")
            .WithDescription("Creates a new user account.");

        group.MapPost(AuthRoutes.SignIn, async (IAuthenticationService authenticationService, 
                    SignInModel model) 
                => await authenticationService.LoginUserAsync(model))
            .WithName("LoginUser")
            .WithSummary("Login user")
            .WithDescription("Authenticates a user and returns a JWT token.");

        return app;
    }

    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup(UserRoutes.Base)
            .WithTags("Users")
            .RequireAuthorization();

        group.MapGet(UserRoutes.Profile, (IUserService userService) =>
                userService.GetUserProfileAsync())
            .WithName("GetUserProfile")
            .WithSummary("Get current user's profile")
            .WithDescription("Returns the profile of the authenticated user.");

        group.MapPut(UserRoutes.Update, (IUserService userService, 
                    UserUpdateModel model) =>
                userService.UpdateUserProfileAsync(model))
            .WithName("UpdateUserProfile")
            .WithSummary("Update current user's profile")
            .WithDescription("Updates basic profile fields for the authenticated user.");

        group.MapDelete(UserRoutes.Delete, (IUserService userService) =>
                userService.DeleteUserProfileAsync())
            .WithName("DeleteUserProfile")
            .WithSummary("Delete current user's profile")
            .WithDescription("Deletes the authenticated user's account.");

        return app;
    }

    public static WebApplication MapWorkoutEndpoints(this WebApplication app)
    {
        var group = app.MapGroup(WorkoutRoutes.Base)
                       .RequireAuthorization()
                       .WithTags("Workouts");
        
        group.MapPost(string.Empty, (IWorkoutService workoutService, 
                    WorkoutModel model, 
                    CancellationToken token) =>
                workoutService.CreateWorkoutAsync(model, token))
            .WithName("CreateWorkout")
            .WithSummary("Create a workout")
            .WithDescription("Creates a new workout for the current user.");
        
        group.MapGet(string.Empty, (IWorkoutService workoutService, 
                    CancellationToken token) =>
                workoutService.GetAllWorkoutsAsync(token))
            .WithName("ListWorkouts")
            .WithSummary("List all workouts")
            .WithDescription("Returns all workouts of the current user.");
        
        group.MapGet(WorkoutRoutes.Search, (IWorkoutService workoutService, 
                    string keyword, 
                    CancellationToken token) =>
                workoutService.SearchWorkoutsByKeywordAsync(keyword, token))
            .WithName("SearchWorkouts")
            .WithSummary("Search workouts by keyword")
            .WithDescription("Returns workouts that match the specified keyword.");

        group.MapPost(WorkoutRoutes.Sorted, (IWorkoutService workoutService,
                    WorkoutSortByDateModel model,
                    CancellationToken token) =>
                workoutService.SortWorkoutsByDateAsync(model, token))
            .WithName("SortWorkoutsByDate")
            .WithSummary("Sort workouts by date")
            .WithDescription(
                "Returns workouts sorted by date according to the provided criteria (ascending/descending).");

        group.MapPut(WorkoutRoutes.ById, (IWorkoutService workoutService,
                    int id,
                    WorkoutModel model,
                    CancellationToken token) =>
                workoutService.UpdateWorkoutAsync(id, model, token))
            .WithName("UpdateWorkout")
            .WithSummary("Update a workout")
            .WithDescription("Updates workout fields by id.");

        group.MapDelete(WorkoutRoutes.ById, (IWorkoutService workoutService,
                    int id,
                    CancellationToken token) =>
                workoutService.DeleteWorkoutAsync(id, token))
            .WithName("DeleteWorkout")
            .WithSummary("Delete a workout")
            .WithDescription("Deletes a workout by id.");

        return app;
    }

    public static WebApplication MapExerciseEndpoints(this WebApplication app)
    {
        var group = app.MapGroup(ExerciseRoutes.Base)
            .WithTags("Exercises")
            .RequireAuthorization();

        group.MapPost(string.Empty, (IExerciseService exerciseService, 
                    ExerciseModel model, 
                    CancellationToken token) =>
                exerciseService.CreateExerciseAsync(model, token))
            .WithName("CreateExercise")
            .WithSummary("Create a new exercise")
            .WithDescription("Creates a new exercise for the current user.");

        group.MapGet(string.Empty, (IExerciseService exerciseService, 
                    CancellationToken token) =>
                exerciseService.GetAllExercisesAsync(token))
            .WithName("ListExercises")
            .WithSummary("Get all exercises")
            .WithDescription("Returns a list of all exercises for the current user.");

        group.MapPut(ExerciseRoutes.ById, (IExerciseService exerciseService, 
                    int id, 
                    ExerciseModel model, 
                    CancellationToken token) =>
                exerciseService.UpdateExerciseAsync(id, model, token))
            .WithName("UpdateExercise")
            .WithSummary("Update an exercise")
            .WithDescription("Updates an existing exercise by its ID.");

        group.MapDelete(ExerciseRoutes.ById, (IExerciseService exerciseService, 
                    int id, 
                    CancellationToken token) =>
                exerciseService.DeleteExerciseAsync(id, token))
            .WithName("DeleteExercise")
            .WithSummary("Delete an exercise")
            .WithDescription("Deletes an existing exercise by its ID.");

        return app;
    }

    public static WebApplication MapExerciseWorkoutEndpoints(this WebApplication app)
    {
        var group = app.MapGroup(WorkoutExerciseRoutes.Base)
            .WithTags("WorkoutExercises")
            .RequireAuthorization();

        group.MapPost(string.Empty, (int workoutId, 
                WorkoutExerciseModel model, 
                IWorkoutExerciseService workoutExerciseService, 
                CancellationToken token) =>
            {
                model.WorkoutId = workoutId;
                return workoutExerciseService.CreateWorkoutExerciseAsync(model, token);
            })
            .WithName("AddExerciseToWorkout")
            .WithSummary("Add exercise to workout")
            .WithDescription("Adds an exercise to the specified workout with sets/reps/weight.");

        group.MapGet(string.Empty, (int workoutId, 
                    IWorkoutExerciseService workoutExerciseService, 
                    CancellationToken token) =>
                workoutExerciseService.GetExercisesForWorkoutAsync(workoutId, token))
            .WithName("ListExercisesForWorkout")
            .WithSummary("List exercises for workout")
            .WithDescription("Returns all exercises for the specified workout.");

        group.MapPut(WorkoutExerciseRoutes.ById, (int workoutId, 
                int exerciseId, 
                WorkoutExerciseModel model, 
                IWorkoutExerciseService workoutExerciseService, 
                CancellationToken token) =>
            {
                model.WorkoutId  = workoutId;
                model.ExerciseId = exerciseId;
                return workoutExerciseService.UpdateWorkoutExerciseAsync(model, token);
            })
            .WithName("UpdateWorkoutExercise")
            .WithSummary("Update workout exercise payload")
            .WithDescription("Updates sets/reps/weight for an exercise within a workout.");

        group.MapDelete(WorkoutExerciseRoutes.ById, (int workoutId, 
                    int exerciseId, 
                    IWorkoutExerciseService workoutExerciseService, 
                    CancellationToken token) =>
                workoutExerciseService.DeleteWorkoutExerciseAsync(workoutId, exerciseId, token))
            .WithName("DeleteWorkoutExercise")
            .WithSummary("Remove exercise from workout")
            .WithDescription("Deletes the exercise from the specified workout.");

        return app;
    }
}