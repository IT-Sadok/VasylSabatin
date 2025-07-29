namespace MyWebApp.DTO.Exceptions;

public class UserUpdateFailedException : Exception
{
    public UserUpdateFailedException(string errorMessages) 
        : base(errorMessages)
    {
    }
}