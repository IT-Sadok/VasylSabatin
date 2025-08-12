namespace MyWebApp.DTO.Exceptions;

public class WeightValidationException : Exception
{
    public WeightValidationException()
        : base("Weight must be  >= 0.")
    {
    }
}