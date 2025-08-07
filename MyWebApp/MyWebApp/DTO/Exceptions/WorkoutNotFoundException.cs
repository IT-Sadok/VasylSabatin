namespace MyWebApp.DTO.Exceptions;

public class WorkoutNotFoundException : Exception
{
    public WorkoutNotFoundException(int workoutId)
        : base($"Workout with ID {workoutId} not found. Please try again.")
    {
    }
}