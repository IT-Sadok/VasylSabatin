namespace MyWebApp.Constants;

public static class WorkoutExerciseRoutes
{
    public const string Root = "";
    
    public const string Base = $"{ApiRoutes.Prefix}/workouts/{RouteParams.workoutId}/exercises";
    
    public const string ById =$"/{RouteParams.exerciseId}";
}