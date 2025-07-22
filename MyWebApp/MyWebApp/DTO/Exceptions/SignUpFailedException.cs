namespace MyWebApp.DTO.Exceptions;

public class SignUpFailedException : Exception
{
    public SignUpFailedException(string errorMessage) 
        : base(errorMessage)
    {
    }
}