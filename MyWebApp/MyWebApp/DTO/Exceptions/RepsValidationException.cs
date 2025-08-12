namespace MyWebApp.DTO.Exceptions;

public class RepsValidationException : Exception
{
    public RepsValidationException(int providedReps)
        : base($"Invalid number of reps entered: {providedReps}. Enter a number >= 1.")
    {
    }
}