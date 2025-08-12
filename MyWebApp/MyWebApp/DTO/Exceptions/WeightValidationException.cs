namespace MyWebApp.DTO.Exceptions;

public class WeightValidationException : Exception
{
    public WeightValidationException(double providedWeight)
        : base($"Invalid number of sets entered: {providedWeight}. Enter a number >= 0.")
    {
    }
}