namespace MyWebApp.DTO.Exceptions;

public class WorkoutExerciseConflictException : Exception
{
    public WorkoutExerciseConflictException(int workoutId, int exerciseId)
        : base("Workout already contains this exercise. Please try again.")
    {
    }
}