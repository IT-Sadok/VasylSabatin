namespace MyWebApp.DTO.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException() 
        : base("User not found. Please try again.")
    {
    }
}