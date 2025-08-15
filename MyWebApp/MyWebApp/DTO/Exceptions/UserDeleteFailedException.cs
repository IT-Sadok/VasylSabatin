namespace MyWebApp.DTO.Exceptions;

public class UserDeleteFailedException : Exception
{
    public UserDeleteFailedException(string errorMessages) 
        : base(errorMessages)
    {
    }
}