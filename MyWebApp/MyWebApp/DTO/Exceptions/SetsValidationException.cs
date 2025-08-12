namespace MyWebApp.DTO.Exceptions;

public class SetsValidationException : Exception
{
    public SetsValidationException(int providedSets)
        : base($"Invalid number of sets entered: {providedSets}. Enter a number >= 1.")
    {
    }
}