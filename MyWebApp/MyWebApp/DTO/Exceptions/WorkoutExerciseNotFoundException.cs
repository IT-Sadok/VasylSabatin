namespace MyWebApp.DTO.Exceptions;

public class WorkoutExerciseNotFoundException : Exception
{
    public WorkoutExerciseNotFoundException(int workoutId, int exerciseId)
    : base($"Workout with ID {workoutId} does not contain exercise with ID {exerciseId}. Please try again.")
    {
    }
}