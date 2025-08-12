namespace MyWebApp.Constants;

public class WorkoutExerciseRoutes
{
    public const string Base = "api/workouts/{workoutId:int}/exercises";
    
    public const string ById = "/{exerciseId:int}";
}