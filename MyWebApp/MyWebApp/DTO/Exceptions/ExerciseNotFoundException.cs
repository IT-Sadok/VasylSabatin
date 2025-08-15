namespace MyWebApp.DTO.Exceptions;

public class ExerciseNotFoundException : Exception
{
    public ExerciseNotFoundException(int exerciseId)
        : base($"Exercise with ID {exerciseId} not found. Please try again.")
    {
    }
}