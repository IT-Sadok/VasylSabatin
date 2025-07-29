namespace MyWebApp.DTO.Exceptions;

public class InvalidTokenException : Exception
{
    public InvalidTokenException() 
        : base("Invalid or expired user token. Please try again.")
    {
    }
}