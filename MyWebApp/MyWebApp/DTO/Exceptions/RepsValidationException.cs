namespace MyWebApp.DTO.Exceptions;

public class RepsValidationException : Exception
{
    public RepsValidationException()
        : base("Reps must be  >= 1.")
    {
    }
}