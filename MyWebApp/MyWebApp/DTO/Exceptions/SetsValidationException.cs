namespace MyWebApp.DTO.Exceptions;

public class SetsValidationException : Exception
{
    public SetsValidationException()
        : base("Reps must be  >= 1.")
    {
    }
}