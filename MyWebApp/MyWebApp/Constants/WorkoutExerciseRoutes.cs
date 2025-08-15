namespace MyWebApp.Constants;

public static class WorkoutExerciseRoutes
{
    public const string Base = $"{ApiRoutes.Prefix}/workouts/{RouteParams.WorkoutId}/exercises";
    
    public const string ById =$"/{RouteParams.ExerciseId}";
}